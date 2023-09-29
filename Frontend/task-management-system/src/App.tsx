import React from "react";
import "./styles/App.css";
import TreeView from "./components/TreeView";
import MainView from "./components/MainView";

function App() {
    return (
        <div className="App">
            <header>
                <div className="header-left-section">
                    <h2>Task Management system</h2>
                </div>
                <div className="header-right-section"></div>
            </header>
            <main>
                <div className="tree-container">
                    <TreeView />
                </div>
                <div className="content">
                    <MainView />
                </div>
            </main>
        </div>
    );
}

export default App;
