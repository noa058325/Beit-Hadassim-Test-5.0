
/* דף מעקב הזמנות עבור בעל מכולת בדיוק כמו של ספק 
רק שהוא מעדכן לסטטוס הושלמה על
ידי הכפתור לאחר שזה במצב של בתהליך...*/ 

import React, { useState, useEffect } from 'react';

const OrdersGrocer = () => {
    const [orders, setOrders] = useState([]);

    useEffect(() => {
        // קריאה ל-API לקבלת ההזמנות
        fetch('https://localhost:64900/api/Order')
            .then(response => response.json())
            .then(data => setOrders(data));
    }, []);

    const handleConfirmOrder = (orderId) => {
        // קריאה ל-API לאישור הזמנה
        fetch(`https://localhost:64900/api/Grocer/confirmOrder`, {
            method: 'PUT',
        }).then(() => {
            // עדכון הסטטוס ל"הושלמה" ברשימה המקומית
            setOrders(orders.map(order =>
                order.id === orderId ? { ...order, status: 'Completed' } : order
            ));
        });
    };

    return (
        <div>
            <h2>מעקב הזמנות</h2>
            <ul>
                {orders.map(order => (
                    <li key={order.id} style={{ backgroundColor: getOrderColor(order.status) }}>
                        <span>{order.productName}</span>
                        <span>{order.status}</span>
                        {order.status === 'In Process' && (
                            <button onClick={() => handleConfirmOrder(order.id)}>
                                אישור תהליך
                            </button>
                        )}
                    </li>
                ))}
            </ul>
        </div>
    );
};

const getOrderColor = (status) => {
    switch (status) {
        case 'Completed':
            return 'green';
        case 'Pending':
            return 'yellow';
        case 'In Process':
            return 'blue';
        default:
            return 'white';
    }
};

export default OrdersGrocer;
