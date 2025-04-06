
/**הדף הראשי מכיל כפתורים עבור כניסת ספק\בעל מכולת */

import React from 'react';
import { useNavigate } from 'react-router-dom';

const HomePage = () => {
  const navigate = useNavigate();

  const handleGrocerClick = () => {
    // מעבר לדף כניסת בעל המכולת
    navigate('/forms/loginGrocer');
  };
  
  const handleSupplierClick = () => {
    // מעבר לדף כניסת ספק או רישום ספק
    navigate('/forms/loginSupplier');
  };

  return (
    <div className="homepage">
      <h1>Welcome to the Store Management System</h1>
      <div className="button-group">
        <button onClick={handleGrocerClick}>Grocer Login</button>
        <button onClick={handleSupplierClick}>Supplier Login</button>
      </div>
    </div>
  );
};

export default HomePage;
