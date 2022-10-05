import { React, useState } from 'react';

const home = () => {
    return (
        <div className="space-y-2">
            <h1 className="text-right text-2xl font-bold">Dashboard</h1>
            <hr/>
            {localStorage.getItem('authToken')}
        </div>
    );
}

export default home;