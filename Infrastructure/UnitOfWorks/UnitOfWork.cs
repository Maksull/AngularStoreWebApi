using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.UnitOfWorks
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;
        private readonly Lazy<ISupplierRepository> _supplierRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;

        public UnitOfWork(Lazy<IProductRepository> productRepository, Lazy<ICategoryRepository> categoryRepository, Lazy<ISupplierRepository> supplierRepository, Lazy<IOrderRepository> orderRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            _orderRepository = orderRepository;
        }

        public IProductRepository Product => _productRepository.Value;

        public ICategoryRepository Category => _categoryRepository.Value;

        public ISupplierRepository Supplier => _supplierRepository.Value;

        public IOrderRepository Order => _orderRepository.Value;
    }
}
