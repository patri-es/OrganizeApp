import ProductCatalog from "../components/product/ProductCatalog";
import ProductFilters from "../components/product/ProductFilters";
import ProductGrid from "../components/product/ProductGrid";

function About() {
    return (
        <main className="mx-auto w-full max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
            <h1 className="mb-6 text-2xl font-bold text-gray-900">
                Explorar productos
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
                    <div className="flex flex-col gap-8 md:flex-row">
                        {/* Barra lateral */}
                        <aside className="w-full shrink-0 md:w-64">
                            <ProductFilters
                                categories={categories}
                                filters={filters}
                                onFiltersChange={onFiltersChange}
                                sortPrice={sortPrice}
                                onSortPriceChange={onSortPriceChange}
                                layout="sidebar"
                            />
                        </aside>

                        {/* Catálogo */}
                        <section className="min-w-0 flex-1">
                            <ProductGrid
                                products={products}
                                loading={loading}
                            />
                        </section>
                    </div>
                )}
            </ProductCatalog>
        </main>
    );
}

export default About;