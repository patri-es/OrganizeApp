import { useState } from 'react';
import { Save } from 'lucide-react';

const PersonForm = () => {

    const [formData, setFormData] = useState({
        name: '',
        lastName: '',
        birthDate: '',
        telephone: ''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData({
            [name]: value
        });
    };

    const handleSubmit = (e) => {
        e.PreventDefault();

        // llamar a la Api
    }

    return (
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
            <form onSubmit={handleSubmit} className="bg-white rounded-xl shadow-lg border border-gray-100 overflow-hidden"> 
                {/* Form Header */} 
                <div className="px-6 py-5 border-b border-gray-100">
                    <h3 className="text-xl font-bold bg-gradient-to-r from-blue-600 to-teal-600 bg-clip-text text-transparent">
                        Person </h3>
                    <div className="h-1 w-16 mt-3 rounded-full bg-gradient-to-r from-blue-600 to-teal-500"></div>
                </div> 
                    {/* Form Fields */} 
                    <div 
                        className="p-6 space-y-5"> 
                    {/* Name */} 
                    <div>
                        <label htmlFor="name"
                    className="block text-sm font-semibold text-gray-700 mb-2"> Name </label> <input type="text"
                        id="name" name="name" value={formData.name} onChange={handleChange} placeholder="Enter name"
                        required
                        className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />
                </div> 
                    {/* LastName */} 
                    <div>
                        <label htmlFor="lastName"
                    className="block text-sm font-semibold text-gray-700 mb-2"> LastName </label> <input type="text"
                        id="lastName" name="lastName" value={formData.lastName} onChange={handleChange}
                        placeholder="Enter last name" required
                        className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />
                    </div> 
                        {/* BirthDate */} 
                        <div>
                            <label htmlFor="birthDate"
                        className="block text-sm font-semibold text-gray-700 mb-2"> BirthDate </label> <input type="date"
                            id="birthDate" name="birthDate" value={formData.birthDate} onChange={handleChange} required
                            className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />
                    </div> 
                        {/* Telephone */} 
                        <div>
                            <label htmlFor="telephone"
                        className="block text-sm font-semibold text-gray-700 mb-2"> Telephone </label> <input type="tel"
                            id="telephone" name="telephone" value={formData.telephone} onChange={handleChange}
                            placeholder="Enter telephone" required
                            className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />
                    </div>
                </div> 
                    {/* Form Footer */} 
                    <div 
                    className="px-6 py-4 bg-gray-50 border-t border-gray-100 flex justify-end">

                    <button type="submit"
                        className="px-4 py-2.5 rounded-full text-sm font-semibold text-white bg-gradient-to-b from-teal-500 via-teal-600 to-teal-500 hover:from-teal-600 hover:to-blue-600 shadow-md hover:shadow-lg transition-all duration-300 transform hover:scale-105">
                        <Save />
                    </button> </div> 
                            {/* Decorative gradient line */} 
                            <div

                                className="h-1 bg-gradient-to-r from-blue-600 via-blue-500 to-teal-500"></div>
            </form>
        </div>
    );
}

export default PersonForm