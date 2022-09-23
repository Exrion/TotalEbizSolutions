import { Sidebar, Footer } from 'flowbite-react';
import React, { useState } from 'react';
import { json, Link } from 'react-router-dom';
import './App.css';

import {
  Routes,
  Route,
} from "react-router-dom";

//Components
import login from './Components/Account/login.component';
import Account from './Components/Account/account.component';
import Home from './Components/Main/home.component';
import Products from './Components/Product/products.component';

import { FaUser, FaHome, FaShoppingBasket } from 'react-icons/fa';

function App() {
  //API
  const apiUrl = 'https://localhost:44394';

  const [message, setMessage] = useState('');

  //Register Form
  //#region Register
  const [usernameRegister, setUsernameRegister] = useState('');
  const [passwordRegister, setPasswordRegister] = useState('');
  const [GivenNameRegister, setGivenNameRegister] = useState('');
  const [SurnameRegister, setSurnameRegister] = useState('');
  const [EmailRegister, setEmailRegister] = useState('');

  let handleSubmitRegister = async (e) => {
    e.preventDefault();
    try {
      let resLogin = fetch(apiUrl, {
        method: "POST",
        body: JSON.stringify({
          UserName: usernameRegister,
          Password: passwordRegister,
          GivenName: GivenNameRegister,
          Surname: SurnameRegister,
          Email: EmailRegister,
          Role: 'Seller'
        })
      });
      let resJsonLogin = await resLogin.json();
      if (resLogin.status === 200) {
        setUsernameRegister('');
        setPasswordRegister('');
        localStorage.setItem('authToken', resJsonLogin.parse().token)
        setMessage('Login Successful');
      }
      else {
        setMessage('Login Unsuccessful');
      }
    }
    catch (e) {
      console.log(e);
    }
  };
  //#endregion

  //Login Form
  //#region Login
  const [usernameLogin, setUsernameLogin] = useState('');
  const [passwordLogin, setPasswordLogin] = useState('');

  let handleSubmitLogin = async (e) => {
    e.preventDefault();
    try {
      let resLogin = fetch(apiUrl, {
        method: "POST",
        body: JSON.stringify({
          UserName: usernameLogin,
          Password: usernameLogin
        })
      });
      let resJsonLogin = await resLogin.json();
      if (resLogin.status === 200) {
        setUsernameLogin('');
        setPasswordLogin('');
        setMessage('Login Successful');
      }
      else {
        setMessage('Login Unsuccessful');
      }
    }
    catch (e) {
      console.log(e);
    }
  };
  //#endregion

  //#region Functions
  const handleWelcome = () => {
    if (checkToken()) {
      return (`Welcome, ${usernameLogin}`);
    }
    else {
      return (`Welcome Guest`);
    }
  }

  const checkToken = () => {
    if (localStorage.getItem('authToken') === "" || localStorage.getItem('authToken') === null) {
      console.log('no token');
      return false;
    }
    else {
      //Check for expired token and remove auth token
    }
    console.log('token');
    return true;
  }
  //#endregion

  return (
    <div className="App" class="container w-full h-full">
      <div class="flex h-full">
        <div class="flex flex-col justify-between h-full">
          <Sidebar>
            <Sidebar.Items>
              <Sidebar.ItemGroup>
                <p CLASS="p-2 text-xl">Welcome [TEMP]</p>
              </Sidebar.ItemGroup>
              <Sidebar.ItemGroup>
                <Link to="/">
                  <Sidebar.Item icon={FaHome}>
                    {/* Home */}
                    Home
                  </Sidebar.Item>
                </Link>
                <Link to="/products">
                  <Sidebar.Item icon={FaShoppingBasket}>
                    {/* Products */}
                    Products
                  </Sidebar.Item>
                </Link>
              </Sidebar.ItemGroup>
            </Sidebar.Items>
          </Sidebar>
          <div class="flex flex-col justify-end">
            <Sidebar>
              <Sidebar.Items>
                <Sidebar.ItemGroup>
                  <Link to="/account">
                    <Sidebar.Item icon={FaUser}>
                      {/* Account */}
                      Login or Register
                    </Sidebar.Item>
                  </Link>
                </Sidebar.ItemGroup>
              </Sidebar.Items>
            </Sidebar>
            <Footer container={true}>
              <Footer.Copyright
                href="https://github.com/Exrion/TotalEbizSolutions"
                by="A project by Titus"
                year={2022}
              />
            </Footer>
          </div>
        </div>
        <div class="w-full">
          <main>
            <Routes>
              <Route exact path='/account' element={<Account/>} />
              <Route exact path='/' element={<Home/>} />
              <Route exact path='/products' element={<Products/>} />
            </Routes>
          </main>
        </div>
      </div>
    </div>
  );

}

export default App;