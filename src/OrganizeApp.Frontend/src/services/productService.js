import api from "./api"

const PRODUCTS_URL = "/products"

// Products

export const getProducts = (filters = {}) => {
    return api.get(PRODUCTS_URL, {
        params: filters
    })
}

export const getProduct = (id) => {
    return api.get(`${PRODUCTS_URL}/${id}`)
}

export const createProduct = (product) => {
    return api.post(PRODUCTS_URL, product)
}

export const updateProduct = (id, product) => {
    return api.patch(`${PRODUCTS_URL}/${id}`, product)
}

export const deleteProduct = (id) => {
    return api.delete(`${PRODUCTS_URL}/${id}`)
}

// Stock

export const increaseStock = (id, quantity) => {
    return api.post(
        `${PRODUCTS_URL}/${id}/stock/increase`,
        { quantity }
    )
}

export const decreaseStock = (id, quantity) => {
    return api.post(
        `${PRODUCTS_URL}/${id}/stock/decrease`,
        { quantity }
    )
}

// Availability

export const activateProduct = (id) => {
    return api.post(`${PRODUCTS_URL}/${id}/activate`)
}

export const deactivateProduct = (id) => {
    return api.post(`${PRODUCTS_URL}/${id}/deactivate`)
}

// Categories

export const assignCategory = (productId, categoryId) => {
    return api.post(
        `${PRODUCTS_URL}/${productId}/categories/${categoryId}`
    )
}

export const removeCategory = (productId, categoryId) => {
    return api.delete(
        `${PRODUCTS_URL}/${productId}/categories/${categoryId}`
    )
}