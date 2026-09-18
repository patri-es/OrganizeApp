import PersonForm from "./PersonForm"
import PersonList from "./PersonList"

const Person = () => {

    const people = [
        { id: 1, name: 'Pepe', lastName: 'Nuñez', birthDate: '1982-05-13 00:00:00.0000000', telephone: '123456789' },
        { id: 1, name: 'Ana', lastName: 'Nuñez', birthDate: '1982-05-13 00:00:00.0000000', telephone: '123455589' },
        { id: 1, name: 'Lola', lastName: 'Nuñez', birthDate: '1982-05-13 00:00:00.0000000', telephone: '116556789' }
    ]

    return (
        <div className="min-h-screen bg-gray-50 py-8">
            <div className="text-center ">
                <h1 className="text-2xl font-bold bg-gradient-to-r from-blue-600 to-teal-600 bg-clip-text text-transparent hover:from-teal-600 hover:to-blue-600 transition-all duration-300 cursor-pointer">
                    Personas de mi BBDD
                </h1>
            </div>
            <PersonForm />
            <PersonList peopleList={people} />
        </div>
    )
}

export default Person