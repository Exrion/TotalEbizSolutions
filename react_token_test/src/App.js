import { Sidebar, Footer, Modal } from 'flowbite-react';
import React, { useState } from 'react';
import { json, Link } from 'react-router-dom';
import './App.css';

import {
  Routes,
  Route,
} from "react-router-dom";

//Components
import Login from './Components/Account/login.component';
import Account from './Components/Account/account.component';
import Home from './Components/Main/home.component';
import _products from './Components/Product/_products.components';

import { FaUser, FaHome, FaShoppingBasket } from 'react-icons/fa';

function App() {
  const [showModal, setShowModal] = useState(false);

  //#region Functions
  //#region Account Modal
  const handleAccount = () => {
    if (checkToken()) {
      return (
        <Link to="/account">
          <Sidebar.Item icon={FaUser}>
            {/* Account */}
            Account
          </Sidebar.Item>
        </Link>
      );
    }
    else {
      return (
        <React.Fragment>
          <Sidebar.Item icon={FaUser}>
            {/* Account */}
            <button onClick={() => setShowModal(true)}>
              Login or Register
            </button>
          </Sidebar.Item>
          <Modal
            show={showModal}
            onClose={() => setShowModal(false)}
          >
            <Modal.Header>
              <h3 className="text-xl font-medium">
                Login or Register
              </h3>
            </Modal.Header>
            <Modal.Body>
              <Login/>
            </Modal.Body>
          </Modal>
        </React.Fragment>
      );
    }
  }
  //#endregion

  const checkToken = () => {
    if (localStorage.getItem('authToken') == "" || localStorage.getItem('authToken') == null) {
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
    <div className="App container w-full h-full">
      <div className="flex h-full">
        <div className="flex flex-col justify-between h-full shadow">
          <Sidebar>
            <Sidebar.Items>
              <Sidebar.ItemGroup>
                <p className="p-2 text-xl">Not Amazon.com</p>
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
                  {handleAccount()}
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
        <div className="w-full">
          <main className="p-6">
            <Routes>
              <Route exact path='/account' element={<Account />} />
              <Route exact path='/' element={<Home />} />
              <Route exact path='/products' element={<_products />} />
            </Routes>
          </main>
        </div>
      </div>
    </div>
  );

}

export default App;