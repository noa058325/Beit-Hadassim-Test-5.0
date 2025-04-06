/* דף מעקב הזמנות לספק בדיוק כמו דף מעקב הזמנות של בעל מכולת רק שאצלו יהיה כפתור לעדכון סטטוס הזמנה שתהיה בתהליך*/

import React, {  useEffect } from 'react';

const OrdersSuplier = () => {
 
    useEffect(() => {
        // קריאה ל-API לקבלת ההזמנות
        fetch('https://localhost:64900/api/Order')
            .then(response => response.json())
            .then(data => setOrders(data));
    }, []);

    const handleConfirmOrder = (orderId) => {
        // קריאה ל-API לאישור הזמנה
        fetch('https://localhost:64900/api/Supplier/approve', {
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

export default OrdersSuplier;
