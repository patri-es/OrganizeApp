import api from "./api"

const CATEGORIES_URL = "/categories"

// Categories

export const getCategories = () => {
    return api.get(CATEGORIES_URL)
}

export const getCategory = (id) => {
    return api.get(`${CATEGORIES_URL}/${ id}`)
}

export const createCategory = (category) => {
    return api.post(CATEGORIES_URL, category)
}

export const updateCategory = (id, category) => {
    return api.put(`${CATEGORIES_URL}/${ id}`, category)
}

export const deleteCategory = (id) => {
    return api.delete(`${CATEGORIES_URL}/${ id}`)
}