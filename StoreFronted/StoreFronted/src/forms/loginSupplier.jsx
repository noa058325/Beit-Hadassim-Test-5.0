import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';

const LoginSupplier = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = (e) => {
        e.preventDefault();

        // לוגיקה לבדיקה אם שם המשתמש והסיסמה נכונים
        if (username === 'supplier' && password === 'password') {
            navigate('/components/ordersSupplier'); // מעבר לדף ההזמנות של הספק
        } else {
            alert('שם משתמש או סיסמה שגויים');
        }
    };

    return (
        <div className="login-container">
            <h1>כניסת ספק</h1>
            <form onSubmit={handleLogin}>
                <div>
                    <label>שם משתמש</label>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
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
                <button type="submit">התחבר</button>
            </form>


            <p>
                משתמש חדש?
                <Link to="/forms/registerpage">הירשם כאן</Link>
            </p>

        </div>
    );
};

export default LoginSupplier;
