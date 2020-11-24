using eRaceSystem.DAL;
using eRaceSystem.Data.Entities;
using eRaceSystem.Data.POCOs.RacingPOCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.BLL.RacingBLL
{
    [DataObject]
    public class RaceDetailController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RaceDetailPOCO> RaceDetailList(int raceid)
        {
            using (var context = new eRaceContext())
            {
                List<RaceDetailPOCO> results = (from x in context.RaceDetails
                                                where x.RaceID == raceid
                                                select new RaceDetailPOCO
                                                {
                                                    RaceDetailID = x.RaceDetailID,                                                    
                                                    Name = x.Member.FirstName + " " + x.Member.LastName,
                                                    RaceFee = x.RaceFee,
                                                    RentalFee = x.RentalFee,
                                                    Placement = x.Place,
                                                    Refunded = x.Refund,
                                                    RefundReason = x.RefundReason,
                                                    CarClass = x.CarID.HasValue ? x.Car.CarClass.CarClassName : "0",
                                                    CarserialNumber = x.CarID.HasValue ? x.CarID : null,
                                                    Comment = x.Comment

                                                }).ToList();
                return results;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RaceResultsPOCO> ReceResults(int raceid)
        {
            using (var context = new eRaceContext())
            {
                List<RaceResultsPOCO> results = (from x in context.RaceDetails
                                                 where x.Refund == false && x.RaceID == raceid
                                                 select new RaceResultsPOCO
                                                 {
                                                     RaceDetailID = x.RaceDetailID,
                                                     Name = x.Member.FirstName + " " + x.Member.LastName,
                                                     PenaltyID = x.PenaltyID,
                                                     RunTime = x.RunTime,
                                                     Placement = 0
                                                 }).ToList();
                return results;
            }
        }
        
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int RaceDetail_Add(RaceDetailPOCO detail,int raceid,int employeeid)
        {
            using (var context = new eRaceContext())
            {
                #region check race Run
                Race currentRace = (from x in context.Races
                                            where x.RaceID == raceid
                                            select x).FirstOrDefault();
                
                if (currentRace.Run.Equals("Y"))
                {
                    throw new Exception("Race is runned, cannot be modified");
                }
                #endregion

                #region check roster count
                int raceCount = (from x in context.RaceDetails
                                 where x.RaceID == raceid
                                 select x).Count();
                if (raceCount >= currentRace.NumberOfCars)
                {
                    throw new Exception("Maxium member reached, cannot add more");
                }
                #endregion
                RaceDetail item = new RaceDetail();
                item.RaceID = raceid;
                item.MemberID = (from x in context.Members
                                 where (x.FirstName + " " + x.LastName).Equals(detail.Name)
                                 select x.MemberID).FirstOrDefault();

                #region check if the member is duplicated
                List<int> existingMembersID = (from x in context.RaceDetails
                                               where x.RaceID == raceid
                                               select x.Member.MemberID).ToList();
                if (existingMembersID.Contains(item.MemberID))
                {
                    throw new Exception("Member already in the race");
                }
                #endregion
                item.RaceFee = detail.RaceFee;
                item.CarID = detail.CarserialNumber;
                #region check if the car is duplicated
                if (item.CarID != null)
                {
                    List<int?> existingCarsID = (from x in context.RaceDetails
                                                 where x.RaceID == raceid
                                                 select x.CarID).ToList();
                    if (existingCarsID.Contains(item.CarID))
                    {
                        throw new Exception("Car already used by another member");
                    }
                }                
                #endregion
                item.RentalFee = (from x in context.Cars
                                  where x.CarID == item.CarID
                                  select x.CarClass.RaceRentalFee).FirstOrDefault();
                item.Refund = detail.Refunded;


                
                #region add invoice
                Invoice newInvoice = new Invoice();
                newInvoice.InvoiceDate = DateTime.Today;
                newInvoice.SubTotal = item.RaceFee + item.RentalFee;
                newInvoice.GST = newInvoice.SubTotal * (decimal)0.05;
                newInvoice.EmployeeID = employeeid;
                context.Invoices.Add(newInvoice);

                #endregion
                #region get invoice id
                
               
                item.InvoiceID = (from x in context.Invoices
                                  orderby x.InvoiceID descending
                                  select x.InvoiceID+1).FirstOrDefault();
                #endregion
                context.RaceDetails.Add(item);

                
                return context.SaveChanges();

                
               
            }

        }
        

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int RaceDetail_Update(RaceDetailPOCO detail)
        {
            using (var context = new eRaceContext())
            {
                RaceDetail item = context.RaceDetails.Find(detail.RaceDetailID);
                int raceid = item.Race.RaceID;
                #region check race Run
                Race currentRace = (from x in context.Races
                                    where x.RaceID == raceid
                                    select x).FirstOrDefault();

                if (currentRace.Run.Equals("Y"))
                {
                    throw new Exception("Race is runned, cannot be modified");
                }
                #endregion
                
                #region check if car is duplicated
                if (detail.CarserialNumber != null && detail.CarserialNumber!=item.CarID)
                {
                    List<int?> existingCarsID = (from x in context.RaceDetails
                                                 where x.RaceID == raceid
                                                 select x.CarID).ToList();                    
                    if (existingCarsID.Contains(detail.CarserialNumber))
                    {
                        throw new Exception("Car already used by another member");
                    }
                }
                item.CarID = detail.CarserialNumber;
                #endregion
                item.Comment = detail.Comment;
                item.Refund = detail.Refunded;
                item.RefundReason = detail.RefundReason;
                #region check one refund limitation
                if (item.Refund == true && detail.Refunded == false)
                {
                    throw new Exception("Member already refunded, cannot be undone");
                }
                #endregion
                #region check refundReason
                if (!string.IsNullOrEmpty(item.RefundReason))
                {
                    item.RefundReason = item.RefundReason.Trim();
                }
                if (item.Refund&&string.IsNullOrEmpty(item.RefundReason))
                {
                    throw new Exception("A refund reason has to added");
                }
                #endregion
                

                item.RentalFee = (from x in context.Cars
                                  where x.CarID == item.CarID
                                  select x.CarClass.RaceRentalFee
                                  ).FirstOrDefault();
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                return context.SaveChanges();
            }
        }

        public void RacePlacement(List<RaceResultsPOCO> raceRasults)
        {
            using (var context = new eRaceContext())
            {
                RaceDetail existingItem;
                #region check if runned
                //existingItem = context.RaceDetails.Find(raceRasults[0].RaceDetailID);
                //int raceid = existingItem.Race.RaceID;
                //Race currentRace = (from x in context.Races
                //                    where x.RaceID == raceid
                //                    select x).FirstOrDefault();

                //if (currentRace.Run.Equals("Y"))
                //{
                //    throw new Exception("Race is runned, cannot be modified");
                //}
                #endregion
                #region penality check
                List<RaceResultsPOCO> removingResultes = new List<RaceResultsPOCO>();
                foreach (RaceResultsPOCO item in raceRasults)
                {
                    if (item.PenaltyID == null && item.RunTime == null)
                    {
                        throw new Exception("Penalty has to be added if no runtime");
                    }
                    if (item.RunTime==null||item.RunTime ==new TimeSpan(0,0,0))
                    {
                        removingResultes.Add(item);
                    }
                }
                for (int i = 0; i < removingResultes.Count; i++)
                {
                    raceRasults.Remove(removingResultes[i]);
                    existingItem = context.RaceDetails.Find(removingResultes[i].RaceDetailID);
                    existingItem.PenaltyID = removingResultes[i].PenaltyID;
                }
                #endregion
                List<RaceResultsPOCO> sortedList = raceRasults.OrderBy(x => x.RunTime.Value.TotalSeconds).ToList();
                for (int i = 1; i <= raceRasults.Count; i++)
                {
                    sortedList[i-1].Placement = i;
                    existingItem = context.RaceDetails.Find(sortedList[i-1].RaceDetailID);
                    existingItem.RunTime = sortedList[i-1].RunTime;
                    existingItem.Place = sortedList[i-1].Placement;
                    existingItem.PenaltyID = sortedList[i - 1].PenaltyID==0?null:sortedList[i-1].PenaltyID;                 
                    
                   
                    context.Entry(existingItem).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
            }
        }
    }
}
