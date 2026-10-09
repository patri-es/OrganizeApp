import { useEffect, useState } from "react"
import toast from "react-hot-toast"

import { getProducts } from "../../services/productService"
import { getErrorMessage } from "../../utils/errorHelper"
import ProductGrid from "./ProductGrid"

function ProductCatalog() {
    const [products, setProducts] = useState([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        const loadProducts = async () => {
            try {
                const { data } = await getProducts()
                setProducts(data)
            } catch (error) {
                console.error(error)
                toast.error(getErrorMessage(error))
            } finally {
                setLoading(false)
            }
        }

        loadProducts()
    }, [])

    return (
        <section className="w-full">
            <ProductGrid
                products={products}
                loading={loading}
            />
        </section>
    )
}

export default ProductCatalog