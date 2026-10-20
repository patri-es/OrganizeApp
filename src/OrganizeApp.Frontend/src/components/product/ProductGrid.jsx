import ProductCard from "./ProductCard"

function ProductGrid({ products = [], loading = false }) {
    if (loading) {
        return (
            <div className="py-10 text-center text-gray-500">
                Cargando productos...
            </div>
        )
    }

    if (products.length === 0) {
        return (
            <div className="py-10 text-center text-gray-500">
                No hay productos para mostrar.
            </div>
        )
    }

    return (
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
            {products.map(product => (
                <ProductCard key={product.id} product={product} />
            ))}
        </div>
    )
}

export default ProductGrid