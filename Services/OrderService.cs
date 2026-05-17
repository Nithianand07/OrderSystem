using Enterprises.Dtos;
using Enterprises.Entities;
using Enterprises.Repositories;

namespace Enterprises.Services
{
    public interface IOrderService
    {
        Task<List<TrnOrderHead>> GetAllAsync();
        Task<TrnOrderHead?> GetAsync(int id);
        Task<TrnOrderHead> CreateAsync(TrnOrderHead head);
        Task<TrnOrderHead> UpdateAsync(TrnOrderHead head);
        Task<TrnOrderDetail?> GetIndividualAsync(int id);
        Task DeleteAsync(int id);
        Task DeleteAllAsync(int id);
        Task<List<PurchaseSalesListDto>>
        PurchaseSalesReportAsync(
            PurchaseSalesReportDto dto
        );
    }

    public class OrderService : IOrderService
    {
        private readonly OrderRepository _repo;
        private readonly ItemRepository _itemRepo;

        public OrderService(OrderRepository repo, ItemRepository itemRepo)
        {
            _repo = repo;
            _itemRepo = itemRepo;
        }

        public Task<List<TrnOrderHead>> GetAllAsync() => _repo.GetAllAsync();
        public Task<TrnOrderHead?> GetAsync(int id) => _repo.GetAsync(id);

        public Task<TrnOrderDetail?> GetIndividualAsync(int id) => _repo.GetIndividualAsync(id);

        public async Task<TrnOrderHead> CreateAsync(
            TrnOrderHead head)
        {
            // VALIDATION
            if (head.OrderDate.Date >
                DateTime.UtcNow.Date)
            {
                throw new InvalidOperationException(
                    "Order date cannot be in the future"
                );
            }
             if (head.OrderType == "Sales")
    {
        foreach (var item in head.Details!)
        {
            var currentItem =
                await _itemRepo.GetAsync(
                    item.ItemID
                );

            if (currentItem == null)
            {
                throw new InvalidOperationException(
                    "Item not found"
                );
            }

            if (
                item.Quantity >
                currentItem.StockQty
            )
            {
                throw new InvalidOperationException(

                    $"Insufficient stock for item : " +

                    $"{currentItem.ItemName}. " +

                    $"Available stock : " +

                    $"{currentItem.StockQty}"

                );
            }
        }
    }

            // PREFIX
            string prefix =
                head.OrderType == "Purchase"
                ? "PO"
                : "SO";

            // LAST ORDER
            var lastOrder =
                await _repo.GetLastOrderAsync(prefix);

            int nextNo = 1;

            if (lastOrder != null)
            {
                string numberPart =
                    lastOrder.OrderCode
                        .Replace(prefix, "");

                nextNo =
                    Convert.ToInt32(numberPart) + 1;
            }

            // ORDER CODE
            head.OrderCode =
                $"{prefix}{nextNo}";

            // TOTALS
            head.ItemAmount =
                head.Details?.Sum(d =>
                    d.ItemAmount
                ) ?? 0;

            head.Discount =
                head.Details?.Sum(d =>
                    d.Discount
                ) ?? 0;

            head.TotalAmount =
                head.ItemAmount -
                head.Discount;


            var created =
                await _repo.CreateAsync(head);

            foreach (
    var item in created.Details!
)
            {
                await _repo
                    .UpdateCurrentStockAsync(

                        item.ItemID,

                        item.Quantity,

                        created.OrderType,

                        "ADD"
                    );
            }
            return created;
        }
        public async Task DeleteAsync(
            int id)
        {
            // =========================
            // GET DETAIL
            // =========================

            var detail =
                await _repo
                    .GetIndividualAsync(id);

            if (detail == null)
            {
                throw new Exception(
                    "Detail not found"
                );
            }

            // =========================
            // DELETE DETAIL
            // =========================

            await _repo.DeleteAsync(id);

            // =========================
            // UPDATE STOCK
            // =========================

            var order =
                await _repo.GetAsync(
                    detail.OrderNo
                );

            if (order != null)
            {
                await _repo
                    .UpdateCurrentStockAsync(

                        detail.ItemID,

                        detail.Quantity,

                        order.OrderType,
                        "REMOVE"
                    );
            }
        }

        public async Task DeleteAllAsync(
    int id)
        {
            // =========================
            // GET ORDER
            // =========================

            var order =
                await _repo.GetAsync(id);

            if (order == null)
            {
                throw new Exception(
                    "Order not found"
                );
            }

            // =========================
            // DELETE ORDER
            // =========================

            await _repo
                .DeleteAllAsync(id);

            // =========================
            // UPDATE STOCK
            // =========================

            foreach (
                var item in order.Details!
            )
            {
                await _repo
                    .UpdateCurrentStockAsync(

                        item.ItemID,

                        item.Quantity,

                        order.OrderType,
                        "REMOVE"
                    );
            }
        }

        public async Task<TrnOrderHead>
 UpdateAsync(
     TrnOrderHead head)
        {
            // =========================
            // VALIDATION
            // =========================

            if (head.OrderDate.Date >
                DateTime.UtcNow.Date)
            {
                throw new InvalidOperationException(
                    "Order date cannot be in the future"
                );
            }

            // =========================
            // CHECK STOCK FOR SALES
            // =========================

            if (head.OrderType == "Sales")
            {
                foreach (var item in head.Details!)
                {
                    var currentItem =
                        await _itemRepo.GetAsync(
                            item.ItemID
                        );

                    if (currentItem == null)
                    {
                        throw new InvalidOperationException(
                            "Item not found"
                        );
                    }

                    if (
                        item.Quantity >
                        currentItem.StockQty
                    )
                    {
                        throw new InvalidOperationException(

                            $"Insufficient stock for item : " +

                            $"{currentItem.ItemName}. " +

                            $"Available stock : " +

                            $"{currentItem.StockQty}"

                        );
                    }
                }
            }

            // =========================
            // GET OLD ORDER
            // =========================

            var oldOrder =
                await _repo.GetAsync(
                    head.OrderNo
                );

            if (oldOrder == null)
            {
                throw new InvalidOperationException(
                    "Order not found"
                );
            }

            // =========================
            // REMOVE OLD STOCK
            // =========================

            foreach (
                var item in oldOrder.Details!
            )
            {
                await _repo
                    .UpdateCurrentStockAsync(

                        item.ItemID,

                        item.Quantity,

                        oldOrder.OrderType,

                        "REMOVE"
                    );
            }

            // =========================
            // CALCULATE TOTALS
            // =========================

            head.ItemAmount =
                head.Details?.Sum(x =>
                    x.ItemAmount
                ) ?? 0;

            head.Discount =
                head.Details?.Sum(x =>
                    x.Discount
                ) ?? 0;

            head.TotalAmount =
                head.ItemAmount -
                head.Discount;

            // =========================
            // UPDATE ORDER
            // =========================

            var updated =
                await _repo
                    .UpdateAsync(head);

            // =========================
            // ADD NEW STOCK
            // =========================

            foreach (
                var item in updated.Details!
            )
            {
                await _repo
                    .UpdateCurrentStockAsync(

                        item.ItemID,

                        item.Quantity,

                        updated.OrderType,

                        "ADD"
                    );
            }

            return updated;
        }
        public async Task<List<PurchaseSalesListDto>>
PurchaseSalesReportAsync(
    PurchaseSalesReportDto dto
)
        {
            // =========================
            // VALIDATION
            // =========================

            if (dto.FromDate.Date >
               dto.ToDate.Date)
            {
                throw new InvalidOperationException(

                    "From Date cannot be greater than To Date"

                );
            }

            return await _repo
                .PurchaseSalesReportAsync(

                    dto.FromDate,

                    dto.ToDate,

                    dto.OrderType
                );
        }
    }
}
