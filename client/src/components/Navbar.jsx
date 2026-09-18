import { Menu, X } from 'lucide-react';
import { User } from 'lucide-react';
import { useState } from 'react';

const Navbar = () => {

    //const activeLink = 'Home';

    const [isMenuOpen, setIsMenuOpen] = useState(false);


    return (
        <header className="bg-white shadow-lg border-b border-gray-100 sticky top-0 z-50">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex justify-between items-center h-16">
                    {/* Brand Logo */}
                    <div className="flex-shrink-0 inline">
                        <h1 className="text-2xl font-bold bg-gradient-to-r from-blue-600 to-teal-600 bg-clip-text text-transparent hover:from-teal-600 hover:to-blue-600 transition-all duration-300 cursor-pointer">
                            <div className="size-8 rounded-full bg-gradient-to-b from-teal-600 via-teal-500 to-teal-400 inline-flex items-center justify-center me-2">
                            <User className="size-6 text-white" />
                        </div>
                            CRUD
                        </h1>
                    </div>

                    {/* Desktop Navigation */}
                    <nav className="hidden md:block">
                        <div className="font-bold text-teal-700 ml-10 flex items-baseline space-x-8">
                            <a
                                to='/'

                                className={({ isActive }) => `px-4 py-2 rounded-full text-sm font-medium transition-all duration-300 transform hover:scale-105 ${isActive
                                    ? 'bg-gradient-to-r from-blue-500 to-teal-500 text-white shadow-lg'
                                    : 'text-gray-700 hover:text-blue-600 hover:bg-blue-50'
                                    }`}
                            >
                                Home
                            </a>

                            <a
                                to='/about'

                                className={({ isActive }) => `px-4 py-2 rounded-full text-sm font-medium transition-all duration-300 transform hover:scale-105 ${isActive
                                    ? 'bg-gradient-to-r from-blue-500 to-teal-500 text-white shadow-lg'
                                    : 'text-gray-700 hover:text-blue-600 hover:bg-blue-50'
                                    }`}
                            >
                                About
                            </a>

                            <a
                                to='/person'

                                className={({ isActive }) => `px-4 py-2 rounded-full text-sm font-medium transition-all duration-300 transform hover:scale-105 ${isActive
                                    ? 'bg-gradient-to-r from-blue-500 to-teal-500 text-white shadow-lg'
                                    : 'text-gray-700 hover:text-blue-600 hover:bg-blue-50'
                                    }`}
                            >
                                Person
                            </a>
                        </div>
                    </nav>

                    {/* Mobile menu button */}
                    <div className="md:hidden">
                        <button
                            onClick={() => setIsMenuOpen(!isMenuOpen)}
                            className="inline-flex items-center justify-center p-2 rounded-md text-gray-700 hover:text-blue-600 hover:bg-blue-50 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-blue-500 transition-colors duration-200"
                            aria-expanded="false"
                        >
                            <span className="sr-only">Open main menu</span>
                            {isMenuOpen ? (
                                <X className="block h-6 w-6" aria-hidden="true" />
                            ) : (
                                <Menu className="block h-6 w-6" aria-hidden="true" />
                            )}
                        </button>
                    </div>
                </div>
            </div>

            {/* Mobile Navigation Menu */}
            <div
                className={`md:hidden transition-all duration-300 ease-in-out ${isMenuOpen
                    ? 'max-h-64 opacity-100 visible'
                    : 'max-h-0 opacity-0 invisible overflow-hidden'
                    }`}
            >
                <div className="px-2 pt-2 pb-3 space-y-1 bg-gray-50 border-t border-gray-200">
                    <a
                        to="/"
                        className={({ isActive }) => `block px-4 py-3 rounded-lg text-base font-medium transition-all duration-200 ${isActive
                            ? 'bg-gradient-to-r from-blue-500 to-teal-500 text-white shadow-md transform scale-105'
                            : 'text-gray-700 hover:text-blue-600 hover:bg-white hover:shadow-sm'
                            }`}
                    >
                        Home
                    </a>

                    <a
                        to="/about"
                        className={({ isActive }) => `block px-4 py-3 rounded-lg text-base font-medium transition-all duration-200 ${isActive
                            ? 'bg-gradient-to-r from-blue-500 to-teal-500 text-white shadow-md transform scale-105'
                            : 'text-gray-700 hover:text-blue-600 hover:bg-white hover:shadow-sm'
                            }`}
                    >
                        About
                    </a>

                    <a
                        to="/person"
                        className={({ isActive }) => `block px-4 py-3 rounded-lg text-base font-medium transition-all duration-200 ${isActive
                            ? 'bg-gradient-to-r from-blue-500 to-teal-500 text-white shadow-md transform scale-105'
                            : 'text-gray-700 hover:text-blue-600 hover:bg-white hover:shadow-sm'
                            }`}
                    >
                        Person
                    </a>
                </div>
            </div>

            {/* Decorative gradient line */}
            <div className="h-1 bg-gradient-to-r from-blue-600 via-blue-500 to-teal-500"></div>
        </header>
    )
}

export default Navbar