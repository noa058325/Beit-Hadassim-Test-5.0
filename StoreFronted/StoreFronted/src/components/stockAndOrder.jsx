/* דף עבור בעל מכולת (רק אצלו יופיע)
זה בעצם דף לבחירת ספק ואז יוצגו הסחורות לפי בחירת הספק
ויוכל להבצע הזמנה שתצורף לאחר אישור לדף מעקב ההזמנות*/ 


import React, { useState, useEffect } from 'react';

const Stockandorder = () => {
    const [suppliers, setSuppliers] = useState([]);
    const [selectedSupplier, setSelectedSupplier] = useState(null);
    const [products, setProducts] = useState([]);
    const [orderQuantity, setOrderQuantity] = useState(1);

    useEffect(() => {
        // קריאה ל-API לקבלת רשימת הספקים
        fetch('http://localhost:64901/api/supplier')
            .then(response => response.json())
            .then(data => setSuppliers(data));
    }, []);

    const handleSupplierChange = (e) => {
        const supplierId = e.target.value;
        setSelectedSupplier(supplierId);
        // קריאה ל-API לקבלת המוצרים של הספק
        fetch(`https://localhost:64900/api/Grocer/order`)
            .then(response => response.json())
            .then(data => setProducts(data));
    };

    const handleOrder = (productId) => {
        const order = {
            productId,
            quantity: orderQuantity,
        };
        // קריאה ל-API לביצוע הזמנה
        fetch(``, {
            method: 'POST',
            body: JSON.stringify(order),
            headers: { 'Content-Type': 'application/json' },
        }).then(() => {
            alert('ההזמנה נוצרה בהצלחה');
        });
    };

    return (
        <div>
            <h2>הזמנת סחורה</h2>
            <label>בחר ספק:</label>
            <select onChange={handleSupplierChange}>
                {suppliers.map(supplier => (
                    <option key={supplier.id} value={supplier.id}>
                        {supplier.name}
                    </option>
                ))}
            </select>

            {selectedSupplier && (
                <div>
                    <h3>מוצרים</h3>
                    <ul>
                        {products.map(product => (
                            <li key={product.id}>
                                <span>{product.name}</span>
                                <span>{product.price} ₪</span>
                                <span>כמות מינימלית: {product.minQuantity}</span>
                                <input
                                    type="number"
                                    value={orderQuantity}
                                    onChange={(e) => setOrderQuantity(e.target.value)}
                                    min={product.minQuantity}
                                />
                                <button onClick={() => handleOrder(product.id)}>
                                    הזמן
                                </button>
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default Stockandorder;
