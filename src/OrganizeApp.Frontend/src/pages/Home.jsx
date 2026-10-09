import ProductCatalog from "../components/product/ProductCatalog"

function Home() {
    return (
        <main className="mx-auto w-full max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
            <h1 className="mb-6 text-2xl font-bold text-gray-900">
                Nuestros productos
            </h1>

            <ProductCatalog />
        </main>
    )
}

export default Home