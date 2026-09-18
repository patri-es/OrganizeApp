import { useEffect, useState } from "react"
import PersonForm from "./PersonForm"
import PersonList from "./PersonList"
import { useForm } from "react-hook-form"


const Person = () => {

    const [people, setPeople] = useState( [
        { id: 1, name: 'Pepe', lastName: 'Nuñez', birthDate: '1982-05-13 00:00:00.0000000', telephone: '123456789' },
        { id: 2, name: 'Ana', lastName: 'Nuñez', birthDate: '1982-05-13 00:00:00.0000000', telephone: '123455589' },
        { id: 3, name: 'Lola', lastName: 'Nuñez', birthDate: '1982-05-13 00:00:00.0000000', telephone: '116556789' }
    ])

    const [editData, setEditData] = useState(null);

    useEffect(() => {
        methods.reset(editData);
    }, [editData])

    const defaultFormValues = {
        name: '',
        lastName: '',
        birthDate: '',
        telephone: ''
    }

    const methods = useForm({
        defaultValues: defaultFormValues
    });

    const handleFormReset = () => {
        methods.reset(defaultFormValues);
    }

    // edit
    const handleFormSubmit = (person) => {
        if (person.id <= 0) {
            console.log("add");
            setPeople((previousPerson) => [...previousPerson, person]);
        } else {
            console.log("edit");
            setPeople((previousPeople) => previousPeople.map(p => p.id === person.id ? person : p));
        }
        methods.reset(defaultFormValues);
    }
        
    //
    const handlePersonEdit = (person) => {
        if (person.id <= 0) {
            console.log("add");
            setPeople((previousPerson) => [...previousPerson, person]);
        } else {
            console.log("edit");
            setPeople((previousPeople) => previousPeople.map(p => p.id === person.id ? person : p));
        }
        methods.reset(defaultFormValues);
    }


    const handlePersonDelete = (person) => {
        if (!confirm(`Seguro que quieres eliminar a la persona: ${person.name} ${person.lastName}?`)) return;

        setPeople((previousPeople) => previousPeople.filter(p=> p.id !== person.id));

        methods.reset(defaultFormValues);


    }

    return (
        <div className="min-h-screen bg-gray-50 py-8">
            {/* <div className="text-center ">
            </div> */}
            <PersonForm methods={methods} onFormReset={handleFormReset} onFormSubmit={handleFormSubmit} />
            <PersonList peopleList={people} onPersonEdit={handlePersonEdit} onPersonDelete={handlePersonDelete} />
        </div>
    )
}

export default Person