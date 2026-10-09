import ProductCatalog from "../components/product/ProductCatalog";
import ProductFilters from "../components/product/ProductFilters";
import ProductGrid from "../components/product/ProductGrid";

function Home() {
    return (
        <main className="mx-auto w-full max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
            <h1 className="mb-6 text-2xl font-bold text-gray-900">
                Nuestros productos
            </h1>

            <ProductCatalog>
                {({
                    products,
                    categories,
                    filters,
                    onFiltersChange,
                    sortPrice,
                    onSortPriceChange,
                    loading
                }) => (
                    <>
                        <div className="mb-6">
                            <ProductFilters
                                categories={categories}
                                filters={filters}
                                onFiltersChange={onFiltersChange}
                                sortPrice={sortPrice}
                                onSortPriceChange={onSortPriceChange}
                                layout="horizontal"
                            />
                        </div>

                        <div className="w-full">
                            <ProductGrid
                                products={products}
                                loading={loading}
                            />
                        </div>
                    </>
                )}
            </ProductCatalog>
        </main>
    );
}

export default Home;