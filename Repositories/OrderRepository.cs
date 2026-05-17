using Enterprises.Data;
using Enterprises.Dtos;
using Enterprises.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enterprises.Repositories
{
    public class OrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TrnOrderHead>>
       GetAllAsync()
        {
            return await _db.TrnOrderHeads

                .Include(h => h.Details)

                .Where(h => !h.IsDelete)

                .ToListAsync();
        }

        public async Task<TrnOrderHead?> GetAsync(int id)
        {
            return await _db.TrnOrderHeads

                .Include(h =>
                    h.Details
                        .Where(d => !d.IsDelete)
                )

                .FirstOrDefaultAsync(h =>
                    h.OrderNo == id
                    &&
                    !h.IsDelete
                );
        }
        public async Task<TrnOrderDetail?> GetIndividualAsync(int id)
        {
            return await _db.TrnOrderDetails
                .FirstOrDefaultAsync(x =>
                    x.OrderDetailNo == id
                    &&
                    x.IsDelete == false
                );
        }
        public async Task<TrnOrderHead?>
GetLastOrderAsync(string prefix)
        {
            return await _db.TrnOrderHeads

                .Where(x =>
                    x.OrderCode.StartsWith(prefix)
                )

                .OrderByDescending(x =>
                    x.OrderNo
                )

                .FirstOrDefaultAsync();
        }

        public async Task<TrnOrderHead> CreateAsync(TrnOrderHead head)
        {
            try
            {
                _db.TrnOrderHeads.Add(head);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return head;
        }
        public async Task DeleteAsync(int id)
        {
            var entity =
                await _db.TrnOrderDetails
                    .FindAsync(id);

            if (entity != null)
            {
                entity.IsDelete = true;
                _db.TrnOrderDetails
                    .Update(entity);

                await _db.SaveChangesAsync();
            }
        }
        public async Task DeleteAllAsync(int orderNo)
        {
            var orderHead =
                await _db.TrnOrderHeads

                    .FirstOrDefaultAsync(x =>
                        x.OrderNo == orderNo
                    );

            if (orderHead != null)
            {
                orderHead.IsDelete = true;
                _db.TrnOrderHeads
                    .Update(orderHead);
            }
            var orderDetails =
                await _db.TrnOrderDetails

                    .Where(x =>
                        x.OrderNo == orderNo
                    )

                    .ToListAsync();

            if (orderDetails.Any())
            {
                foreach (var item in orderDetails)
                {
                    item.IsDelete = true;

                }

                _db.TrnOrderDetails
                    .UpdateRange(orderDetails);
            }
            await _db.SaveChangesAsync();
        }
        public async Task<TrnOrderHead>
UpdateAsync(TrnOrderHead head)
        {
            var existing =
                await _db.TrnOrderHeads

                    .Include(x => x.Details)

                    .FirstOrDefaultAsync(x =>
                        x.OrderNo == head.OrderNo
                    );

            if (existing == null)
                throw new Exception(
                    "Order not found"
                );

            // =========================
            // HEADER
            // =========================

            existing.OrderDate =
                head.OrderDate;

            existing.OrderType =
                head.OrderType;

            existing.AccountID =
                head.AccountID;

            existing.Reference =
                head.Reference;

            existing.Narration =
                head.Narration;

            existing.ItemAmount =
                head.ItemAmount;

            existing.Discount =
                head.Discount;

            existing.TotalAmount =
                head.TotalAmount;

            // =========================
            // DETAILS
            // =========================

            foreach (var item in head.Details!.ToList())
            {
                // EXISTING DETAIL
                if (item.OrderDetailNo > 0)
                {
                    var existingDetail =
                        existing.Details!
                            .FirstOrDefault(x =>
                                x.OrderDetailNo ==
                                item.OrderDetailNo
                            );

                    if (existingDetail != null)
                    {
                        existingDetail.ItemID =
                            item.ItemID;

                        existingDetail.Quantity =
                            item.Quantity;

                        existingDetail.Rate =
                            item.Rate;

                        existingDetail.Discount =
                            item.Discount;

                        existingDetail.ItemAmount =
                            item.ItemAmount;

                        existingDetail.TotalAmount =
                            item.TotalAmount;
                    }
                }

                // NEW DETAIL
                else
                {
                    existing.Details!.Add(
                        new TrnOrderDetail
                        {
                            ItemID = item.ItemID,

                            Quantity = item.Quantity,

                            Rate = item.Rate,

                            Discount = item.Discount,

                            ItemAmount = item.ItemAmount,

                            TotalAmount = item.TotalAmount
                        }
                    );
                }
            }

            await _db.SaveChangesAsync();

            // =========================
// UPDATE STOCK
// =========================

var itemIds =
    existing.Details!

        .Select(x => x.ItemID)

        .Distinct()

        .ToList();

return existing;
        }
        public async Task
        UpdateCurrentStockAsync(
            int itemID,
            decimal qty,
            string orderType,
            string actionType
        )
        {
            await _db.Database
                .ExecuteSqlRawAsync(

                    "EXEC SP_UpdateCurrentStock @p0, @p1, @p2, @p3",

                    itemID,

                    qty,

                    orderType,

                    actionType
                );
        }
        public async Task<List<PurchaseSalesListDto>>
            PurchaseSalesReportAsync(
                DateTime fromDate,
                DateTime toDate,
                string orderType
            )
        {
            var data =
                await (

                    from h in _db.TrnOrderHeads

                    join a in _db.MstAccounts
                        on h.AccountID equals a.AccountID

                    where

                        h.OrderDate.Date >=
                            fromDate.Date

                        &&

                        h.OrderDate.Date <=
                            toDate.Date

                        &&

                        h.OrderType ==
                            orderType

                        &&

                        h.IsDelete == false

                    select new PurchaseSalesListDto
                    {
                        OrderNo =
                            h.OrderNo,

                        OrderCode =
                            h.OrderCode,

                        OrderDate =
                            h.OrderDate,

                        OrderType =
                            h.OrderType,

                        AccountName =
                            a.AccountName,

                        Narration =
                            h.Narration,

                        Reference =
                            h.Reference,

                        Amount =
                            h.TotalAmount
                    }

                )

                .OrderBy(x =>
                    x.OrderDate
                )

                .ToListAsync();

            return data;
        }
    }
}
