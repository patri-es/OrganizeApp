import { Save, RotateCcw } from 'lucide-react'; //RotateCcw

const PersonForm = ({ methods, onFormReset }) => {

    

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = methods;

 
    const onFormSubmit = (data) => {
        console.log(data);
    }

    return (
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
            <form
                className="bg-white rounded-xl shadow-lg border border-gray-100 overflow-hidden"
                onSubmit={handleSubmit(onFormSubmit)}> 
                <input type="hidden" {...register("id")}
                    id="id" name="id"

                />
                {/* Form Header */} 
                <div className="px-6 py-5 border-b border-gray-100">
                    <h3 className="text-xl font-bold bg-gradient-to-r from-blue-600 to-teal-600 bg-clip-text text-transparent">
                        Add Person </h3>
                    <div className="h-1 w-16 mt-3 rounded-full bg-gradient-to-r from-blue-600 to-teal-500"></div>
                </div> 
                    {/* Form Fields */} 
                    <div 
                        className="p-6 space-y-5"> 
                    {/* Name */} 
                    <div>
                        <label htmlFor="name"
                            className="block text-sm font-semibold text-gray-700 mb-2"> Name </label> 
                        <input type="text"
                            {...register("name", {
                                required: true,
                                maxLength: 50
                            })}
                            
                            placeholder="Enter name"
                        required
                            className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />

                        {errors.name?.type === 'required' && <p className="mt-1 text-sm text-red-600 flex items-center">
                            name is  required
                        </p>}

                        {errors.name?.type === 'maxLength' && <p className="mt-1 text-sm text-red-600 flex items-center">
                            name can not exceed 50 characters
                        </p>}

                </div> 
                    {/* LastName */} 
                    <div>
                        <label htmlFor="lastName"
                            className="block text-sm font-semibold text-gray-700 mb-2"> LastName </label> 
                        <input type="text"
                            id="lastName" name="lastName"
                            {...register("lastName", {
                                required: true,
                                maxLength: 50
                            })}

                        placeholder="Enter last name" required
                            className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />

                        {errors.lastName?.type === 'required' && <p className="mt-1 text-sm text-red-600 flex items-center">
                            lastName is  required
                        </p>}

                        {errors.lastName?.type === 'maxLength' && <p className="mt-1 text-sm text-red-600 flex items-center">
                            lastName can not exceed 50 characters
                        </p>}
                    </div> 
                        {/* BirthDate */} 
                        <div>
                            <label htmlFor="birthDate"
                                className="block text-sm font-semibold text-gray-700 mb-2"> BirthDate </label> 
                            <input type="date"
                                id="birthDate" name="birthDate"
                            {...register("birthDate", {
                                    required: false,
                                    maxLength: 30
                                })}
                            className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />

                            {errors.birthDate?.type === 'maxLength' && <p className="mt-1 text-sm text-red-600 flex items-center">
                            birthDate can not exceed 50 characters
                        </p>}
                    </div> 
                        {/* Telephone */} 
                        <div>
                            <label htmlFor="telephone"
                                className="block text-sm font-semibold text-gray-700 mb-2"> Telephone </label> 
                            <input type="tel"
                                id="telephone" name="telephone"
                            {...register("telephone", {
                                    required: false,
                                    maxLength: 12
                                })}
                                placeholder="Enter telephone"
                                className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />
                    </div>
                </div> 
                    {/* Form Footer */} 
                    <div 
                    className="px-6 py-4 bg-gray-50 border-t border-gray-100 flex justify-end">

                    <button type="submit"
                        className="px-4 py-2.5 rounded-full text-sm font-semibold text-white bg-gradient-to-b from-teal-500 via-teal-600 to-teal-500 hover:from-teal-600 hover:to-blue-600 shadow-md hover:shadow-lg transition-all duration-300 transform hover:scale-105">
                        <Save />
                    </button>
                    <button
                        type="button"
                        onClick={onFormReset}
                        className="ms-3 px-4 py-2.5 rounded-full text-sm font-semibold text-white bg-gradient-to-b from-blue-400 via-blue-600 to-blue-400  hover:from-teal-600 hover:to-blue-600 shadow-md hover:shadow-lg transition-all duration-300 transform hover:scale-105"
                    >
                        <RotateCcw />
                    </button>
                </div> 
                    <div className="h-1 bg-gradient-to-r from-blue-600 via-blue-500 to-teal-500"></div>
            </form>
        </div>
    );
}

export default PersonForm