import React, { useState, Component } from 'react';
import { Label, Tabs, TextInput, Button, Alert } from 'flowbite-react';

const Login = () => {
    //API
    const apiUrl = 'https://localhost:44394';

    const [tabLogin, setTabLogin] = useState(true);
    const [tabRegister, setTabRegister] = useState(false);

    //#region Alert Message
    const [message, setMessage] = useState("");

    const alertMessage = () => {
        if (message) {
            return (
                <Alert
                    color="info"
                    rounded={true}
                    onDismiss={() => setMessage("")}
                >
                    {message}
                </Alert>
            );
        }
        else {
            return ("");
        }
    }
    //#endregion

    //#region Active Tabs
    const activeTab = (tab) => {
        switch (tab) {
            case "Login":
                inactiveTab();
                clearFields();//Notfunctional
                setMessage("");//Notfunctional
                setTabLogin(true);
                break;
            case "Register":
                inactiveTab();
                clearFields();//Notfunctional
                setMessage("");//Notfunctional
                setTabRegister(true);
                break;
        }

    }

    //Sets all tabs to inactive
    const inactiveTab = () => {
        setTabLogin(false);
        setTabRegister(false);
    }

    const clearFields = () => {
        //TODO: Set ID for input fields to properly clear
        // setUsernameLogin("");
        // setPasswordLogin("");
        // setUsernameRegister("");
        // setPasswordRegister("");
        // setGivenNameRegister("");
        // setSurnameRegister("");
        // setEmailRegister("");
        var elements = document.getElementsByName("txtInput");
        for (var i = 0; i < elements.length; i++) {
            elements[i].value = "";
        }
    }
    //#endregion

    //Register Form
    //#region Register
    const [usernameRegister, setUsernameRegister] = useState('');
    const [passwordRegister, setPasswordRegister] = useState('');
    const [GivenNameRegister, setGivenNameRegister] = useState('');
    const [SurnameRegister, setSurnameRegister] = useState('');
    const [EmailRegister, setEmailRegister] = useState('');
    const [RoleRegister] = useState('Seller');

    let handleSubmitRegister = async (e) => {
        e.preventDefault();
        try {
            let resRegister = fetch(`${apiUrl}/api/account/register`, {
                method: "POST",
                headers: new Headers({ 'Content-Type': 'application/json' }),
                body: JSON.stringify({
                    UserName: usernameRegister,
                    Password: passwordRegister,
                    GivenName: GivenNameRegister,
                    Surname: SurnameRegister,
                    Email: EmailRegister,
                    Role: RoleRegister
                })
            });
            if ((await resRegister).ok) {
                clearFields();
                setMessage("Registration Successful");
            }
            else {
                setMessage(`HTTP Error: ${(await resRegister).status} - ${(await resRegister).statusText}`);
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
            let resLogin = fetch(`${apiUrl}/api/account/login`, {
                method: "POST",
                headers: new Headers({ 'Content-Type': 'application/json' }),
                body: JSON.stringify({
                    UserName: usernameLogin,
                    Password: passwordLogin
                })
            });
            if ((await resLogin).ok) {
                clearFields();
                setMessage('Login Successful');
                console.log(JSON.parse((await resLogin).body));
                //localStorage.setItem("authToken", "");
            }
            else {
                setMessage(`HTTP Error: ${(await resLogin).status} - ${(await resLogin).statusText}`);
            }
        }
        catch (e) {
            console.log(e);
        }
    };
    //#endregion

    return (
        <div>
            <Tabs.Group style="underline">
                {/* Login Tab */}
                <Tabs.Item
                    title="Login"
                    onClick={() => activeTab("Login")}
                    active={tabLogin}
                >
                    {/* Login Form */}
                    {alertMessage()}
                    <form
                        className="flex flex-col p-2 gap-4"
                        onSubmit={handleSubmitLogin}
                    >
                        <div>
                            <div className="mb-2 block">
                                <Label
                                    htmlFor="usernameLogin"
                                    value="Username"
                                />
                            </div>
                            <TextInput
                                id="txtUsernameLogin"
                                type="text"
                                placeholder="johndoe420"
                                required={true}
                                shadow={true}
                                onChange={(e) => setUsernameLogin(e.target.value)}
                                name="txtInput"
                            />
                        </div>
                        <div>
                            <div className="mb-2 block">
                                <Label
                                    htmlFor="passwordLogin"
                                    value="Password"
                                />
                            </div>
                            <TextInput
                                id="txtPasswordLogin"
                                type="password"
                                placeholder="A strong password hopefully!"
                                required={true}
                                shadow={true}
                                onChange={(e) => setPasswordLogin(e.target.value)}
                                name="txtInput"
                            />
                        </div>
                        <Button type="submit">
                            Login
                        </Button>
                    </form>
                </Tabs.Item>
                {/* Register Tab */}
                <Tabs.Item
                    title="Register"
                    onClick={() => activeTab("Register")}
                    active={tabRegister}
                >
                    {/* Register Form */}
                    {alertMessage()}
                    <form
                        className="flex flex-col p-2 gap-4"
                        id="formRegister"
                        onSubmit={handleSubmitRegister}
                    >
                        <div>
                            <div className="mb-2 block">
                                <Label
                                    htmlFor="usernameRegister"
                                    value="Username"
                                />
                            </div>
                            <TextInput
                                id="txtUsernameRegister"
                                type="text"
                                placeholder="johndoe420"
                                required={true}
                                shadow={true}
                                onChange={(e) => setUsernameRegister(e.target.value)}
                                name="txtInput"
                            />
                        </div>
                        <div>
                            <div className="mb-2 block">
                                <Label
                                    htmlFor="passwordRegister"
                                    value="Password"
                                />
                            </div>
                            <TextInput
                                id="txtPasswordRegister"
                                type="password"
                                placeholder="A strong password hopefully!"
                                required={true}
                                shadow={true}
                                onChange={(e) => setPasswordRegister(e.target.value)}
                                name="txtInput"
                            />
                        </div>
                        <div>
                            <div className="mb-2 block">
                                <Label
                                    htmlFor="EmailRegister"
                                    value="Email"
                                />
                            </div>
                            <TextInput
                                id="txtEmailRegister"
                                type="email"
                                placeholder="john.doe@totalebizsolutions.com"
                                required={true}
                                shadow={true}
                                onChange={(e) => setEmailRegister(e.target.value)}
                                name="txtInput"
                            />
                        </div>
                        <div>
                            <div className="mb-2 block">
                                <Label
                                    htmlFor="GivenNameRegister"
                                    value="First Name"
                                />
                            </div>
                            <TextInput
                                id="txtGivenNameRegister"
                                type="text"
                                placeholder="John"
                                required={true}
                                shadow={true}
                                onChange={(e) => setGivenNameRegister(e.target.value)}
                                name="txtInput"
                            />
                        </div>
                        <div>
                            <div className="mb-2 block">
                                <Label
                                    htmlFor="SurnameRegister"
                                    value="Last Name"
                                />
                            </div>
                            <TextInput
                                id="txtSurnameRegister"
                                type="text"
                                placeholder="Doe"
                                required={true}
                                shadow={true}
                                onChange={(e) => setSurnameRegister(e.target.value)}
                                name="txtInput"
                            />
                        </div>
                        <Button type="submit">
                            Register
                        </Button>
                    </form>
                </Tabs.Item>
            </Tabs.Group>
        </div>
    );
}

export default Login;