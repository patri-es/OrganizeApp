import PersonForm from "./PersonForm"
import PersonList from "./PersonList"

const Person = () => {

    

    return (
        <div className="min-h-screen bg-gray-50 py-8">
            <div className="text-center ">
                <h1 className="text-2xl font-bold bg-gradient-to-r from-blue-600 to-teal-600 bg-clip-text text-transparent hover:from-teal-600 hover:to-blue-600 transition-all duration-300 cursor-pointer">
                    Personas de mi BBDD
                </h1>
            </div>
            <PersonForm />
            <PersonList />
        </div>
    )
}

export default Person