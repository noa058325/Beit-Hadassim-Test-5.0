import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';

import LoginGrocer from './forms/loginGrocer.jsx';
import HomePageGrocer from './components/homePageGrocer.jsx';
import LoginSupplier from './forms/loginSupplier.jsx';
import SupplierRegister from './forms/registerpage.jsx';
import OrdersSuplier from './components/ordersSupplier.jsx'
import Stockandorder from './components/stockAndOrder.jsx'
import OrdersGrocer from './components/ordersGrocer.jsx';
import HomePage from './homepage.jsx' ;




function App() {
  return (
    <Router>
      <Routes>
        {/* ברירת מחדל - מפנה לדף התחברות */}
        <Route path="/" element={<HomePage />} />

        {/* דף הרשמה ספק */}
        <Route path="/forms/registerpage" element={<SupplierRegister />} />

           {/* ספק דף התחברות */}
           <Route path="/forms/loginSupplier" element={< LoginSupplier/>} />

                 {/* ספק דף התחברות */}
           <Route path="/forms/loginGrocer" element={<LoginGrocer />} />

        {/* דף הבית של בעל המכולת */}
        <Route path="/components/homePageGrocer" element={<HomePageGrocer />} />

        {/* דף יצירת הזמנה */}
        <Route path="/components/stockAndOrder" element={<Stockandorder />} />

        {/* דף צפייה בהזמנות */}
        <Route path="/components/ordersGrocer" element={<OrdersGrocer />} />


        {/* דף צפייה בהזמנות */}
        <Route path="/components/ordersSupplier" element={<OrdersSuplier />} />

      </Routes>
    </Router>
  );
}

export default App;
