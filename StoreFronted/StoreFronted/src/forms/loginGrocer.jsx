import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const LoginGrocer = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate(); 

    const handleLogin = (e) => {
        e.preventDefault();

        // כאן תוכל להוסיף את הלוגיקה להתחברות
        if (username === 'נועה' && password === '1111') {
            navigate('/components/homePageGrocer'); 
        } else {
            alert('שם משתמש או סיסמה שגויים');
        }
    };

    return (
        <div className="login-container">
            <h1> Login</h1>
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
        </div>
    );
};

export default LoginGrocer;
