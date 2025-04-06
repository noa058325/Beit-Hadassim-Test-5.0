/**דף הבית לבעל המכולת בלבד! 
 * כי הספק נכנס רק למעקב הזמנות
 */


import React from 'react';
import { Link } from 'react-router-dom';

const HomePageGrocer = () => {
    return (
        <div className="home-container">
            
            <div className="button-group">
                <Link to="/components/orderspage">
                    <button>מעקב הזמנות</button>
                </Link>
                <Link to="/components/orderpage">
                    <button>הזמנת סחורה</button>
                </Link>
            </div>
        </div>
    );
};

export default HomePageGrocer;