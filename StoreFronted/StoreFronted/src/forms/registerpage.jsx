import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const SupplierRegister = () => {
    const [companyName, setCompanyName] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [password, setPassword] = useState('');
    const [representativeName, setRepresentativeName] = useState('');
    const [products, setProducts] = useState([{ name: '', price: '', minQuantity: '' }]);
    const navigate = useNavigate();

    // הוספת מוצר נוסף
    const handleAddProduct = () => {
        setProducts([...products, { name: '', price: '', minQuantity: '' }]);
    };

    // עדכון מוצר קיים
    const handleProductChange = (index, field, value) => {
        const updatedProducts = [...products];
        updatedProducts[index][field] = value;
        setProducts(updatedProducts);
    };

    // שליחת נתונים לשרת
    const handleRegister = async (e) => {
        e.preventDefault();

        // יצירת אובייקט עם הנתונים של הספק
        const supplierData = {
            companyName,
            phoneNumber,
            password,
            representativeName,
            stocks: products.map(product => ({
                name: product.name,
                price: parseFloat(product.price),
                minQuantity: parseInt(product.minQuantity)
            }))
        };

        try {
            const response = await axios.post('https://localhost:64900/api/Supplier/register', supplierData);
            navigate('/components/ordersSupplier');
        } catch (error) {
            console.error('שגיאה בהרשמה:', error);
            alert('ההרשמה נכשלה. נסה שוב מאוחר יותר.');
        }
    };

    return (
        <div className="register-container">
            <h1>הרשמת ספק</h1>
            <form onSubmit={handleRegister}>
                <div>
                    <label>שם חברה</label>
                    <input
                        type="text"
                        value={companyName}
                        onChange={(e) => setCompanyName(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>מספר טלפון</label>
                    <input
                        type="text"
                        value={phoneNumber}
                        onChange={(e) => setPhoneNumber(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>שם נציג</label>
                    <input
                        type="text"
                        value={representativeName}
                        onChange={(e) => setRepresentativeName(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>סיסמה</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>

                <h3>הוספת מוצרים</h3>
                {products.map((product, index) => (
                    <div key={index} className="product-input">
                        <input
                            type="text"
                            placeholder="שם מוצר"
                            value={product.name}
                            onChange={(e) => handleProductChange(index, 'name', e.target.value)}
                            required
                        />
                        <input
                            type="number"
                            placeholder="מחיר לפריט"
                            value={product.price}
                            onChange={(e) => handleProductChange(index, 'price', e.target.value)}
                            required
                        />
                        <input
                            type="number"
                            placeholder="כמות מינימלית"
                            value={product.minQuantity}
                            onChange={(e) => handleProductChange(index, 'minQuantity', e.target.value)}
                            required
                        />
                    </div>
                ))}
                <button type="button" onClick={handleAddProduct}>הוסף מוצר</button>

                <button type="submit">הירשם</button>
            </form>
        </div>
    );
};

export default SupplierRegister;
