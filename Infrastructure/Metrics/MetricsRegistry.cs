using App.Metrics;
using App.Metrics.Counter;

namespace Infrastructure.Metrics
{
    public static class MetricsRegistry
    {

        #region Products

        public static CounterOptions GetProductsCounter => new()
        {
            Name = "Products requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions GetProductByIdCounter => new()
        {
            Name = "Product by id requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions CreateProductCounter => new()
        {
            Name = "Created Product count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions UpdateProductCounter => new()
        {
            Name = "Updated Product count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions DeleteProductCounter => new()
        {
            Name = "Deleted Product count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };

        #endregion

        #region Categories

        public static CounterOptions GetCategoriesCounter => new()
        {
            Name = "Categories requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions GetCategoryByIdCounter => new()
        {
            Name = "Category by id requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions CreateCategoryCounter => new()
        {
            Name = "Created Category count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions UpdateCategoryCounter => new()
        {
            Name = "Updated Category count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions DeleteCategoryCounter => new()
        {
            Name = "Deleted Category count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };

        #endregion

        #region Suppliers

        public static CounterOptions GetSuppliersCounter => new()
        {
            Name = "Suppliers requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions GetSupplierByIdCounter => new()
        {
            Name = "Supplier by id requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions CreateSupplierCounter => new()
        {
            Name = "Created Supplier count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions UpdateSupplierCounter => new()
        {
            Name = "Updated Supplier",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions DeleteSupplierCounter => new()
        {
            Name = "Deleted Supplier count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };

        #endregion

        #region Orders

        public static CounterOptions GetOrdersCounter => new()
        {
            Name = "Orders requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions GetOrderByIdCounter => new()
        {
            Name = "Order by id requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions GetOrderByUserIdCounter => new()
        {
            Name = "Order by user id requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions CreateOrderCounter => new()
        {
            Name = "Created Order count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions UpdateOrderCounter => new()
        {
            Name = "Updated Order count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions DeleteOrderCounter => new()
        {
            Name = "Deleted Order count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };


        #endregion

        #region Auth

        public static CounterOptions LoginCounter => new()
        {
            Name = "User logged in count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions RegisterCounter => new()
        {
            Name = "User registered count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions GetUserDataCounter => new()
        {
            Name = "User data requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions ConfirmEmailCounter => new()
        {
            Name = "Confirm email requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions RefreshCounter => new()
        {
            Name = "Refresh jwt token requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions ResetPasswordCounter => new()
        {
            Name = "Reset password requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions ConfirmResetPasswordCounter => new()
        {
            Name = "Confirm reset password requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };

        #endregion

        #region Ratings

        public static CounterOptions GetRatingsCounter => new()
        {
            Name = "Ratings requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions GetRatingByIdCounter => new()
        {
            Name = "Rating by id requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions GetRatingByUserIdCounter => new()
        {
            Name = "Rating by user id requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions GetRatingByProductIdCounter => new()
        {
            Name = "Rating by product id requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions CreateRatingCounter => new()
        {
            Name = "Created Rating count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions UpdateRatingCounter => new()
        {
            Name = "Updated Rating count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };
        public static CounterOptions DeleteRatingCounter => new()
        {
            Name = "Deleted Rating count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };

        #endregion

        #region Image

        public static CounterOptions GetImageCounter => new()
        {
            Name = "Image requested count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };

        public static CounterOptions UploadImageCounter => new()
        {
            Name = "Image uploaded count",
            Context = "AngularStoreApi",
            MeasurementUnit = Unit.Calls,
        };

        #endregion
    }
}
