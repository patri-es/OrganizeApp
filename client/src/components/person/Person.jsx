import { useEffect, useState } from "react"
import PersonForm from "./PersonForm"
import PersonList from "./PersonList"
import { useForm } from "react-hook-form"
import toast from "react-hot-toast"
import axios from "axios"


const Person = () => {
    const BASE_URL = import.meta.env.VITE_BASE_API_URL + '/people';

    const [people, setPeople] = useState([]);
    const [loading, setLoading] = useState(true);
    const [editData, setEditData] = useState(null);


    useEffect(() => {
        try {
            const loadPeople = async () => {
                var peopleData = (await axios.get(BASE_URL)).data;
                setPeople(peopleData);
            }
            loadPeople();

        } catch (error) {
            console.log(error);
            toast.error("Error has occured!");
        }
        finally {
            setLoading(false);
        }

    }, []);

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

    // reset
    const handleFormReset = () => {
        methods.reset(defaultFormValues);
    }

    // Submit
    const handleFormSubmit = async (person) => {
        setLoading(true);
        try {
            if (person.id <= 0) {
                const createdPerson = (await axios.post(BASE_URL, person).data);
                setPeople((previousPerson) => [...previousPerson, createdPerson]);
            } else {
                await axios.post(`${BASE_URL}/${person.id}`, person);
                setPeople((previousPeople) => previousPeople.map(p =>
                    p.id === person.id ? person : p));
            }
            methods.reset(defaultFormValues);
            toast.success("Guardado todo!");

        } catch (error) {
            console.log(error);
            toast.error("No Guardado ERROR!");
        }
        finally {
            setLoading(false);
        }
    }

    // edit
    const handlePersonEdit = (person) => {
        setEditData(person);
    }
            
    // delete
    const handlePersonDelete = async (person) => {
        if (!confirm(`Seguro que quieres eliminar a la persona: ${person.name} ${person.lastName}?`)) return;
        setLoading(true);

        try {
            setPeople((previousPeople) => previousPeople.filter(p => p.id !== person.id));
            await axios.post(`${BASE_URL}/${person.id}`, person);
            toast.success("Borrado perfect!");

        } catch (error) {
            console.log(error);
            toast.error("No Borrado ERROR!");
        }
        finally {
            setLoading(false);
        }
    }

    return (
        <div className="min-h-screen bg-gray-50 py-8">
           <p>URL: { BASE_URL}</p>
            <p>
                {loading && <span>Loading...</span>}
            </p>
            {/* <div className="text-center ">
            </div> */}
            <PersonForm methods={methods} onFormSubmit={handleFormSubmit} onFormReset={handleFormReset} />
            <PersonList peopleList={people} onPersonEdit={handlePersonEdit} onPersonDelete={handlePersonDelete} />
        </div>
    )
}

export default Person