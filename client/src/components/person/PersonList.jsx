import { Edit, Trash2 } from 'lucide-react'

const PersonList = ({ peopleList, onPersonEdit, onPersonDelete }) => {


    return (

        <div>
            <ul>
                {
                    peopleList.map(person => <li key={person.id} 
                        className="">
                        <div>
                            <span>{ person.name} </span>
                            <span>{person.lastName} </span>
                        </div>
                        <div className="flex items-center justify-center space-x-2">
                             <button onClick={() => onPersonEdit(person)} 
                                className="inline-flex items-center px-3 py-2 bg-blue-100 text-blue-700 text-sm font-medium rounded-lg hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105"
                                title="Edit person"
                            >
                                <Edit className="w-4 h-4 mr-1" />
                                Edit
                            </button>
                            <button
                                 onClick={() => onPersonDelete(person)} 


                                className="inline-flex items-center px-3 py-2 bg-red-100 text-red-700 text-sm font-medium rounded-lg hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105"
                                title="Delete person"
                            >
                                <Trash2 className="w-4 h-4 mr-1" />
                                Delete
                            </button>
                        </div>
                    </li>
                        
                    )
                }

                <li></li>


            </ul>
        </div>

    )
}

export default PersonList