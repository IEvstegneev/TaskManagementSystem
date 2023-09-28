import React, { useState } from "react";
import "./styles/App.css";
import TreeView from "./components/TreeView";
import IssueForm from "./components/IssueForm";
import ModalWindow from "./components/ModalWindow/ModalWindow";
import IssueView from "./components/IssueView";

function App() {
    const [count, setCount] = useState(0);
    const [text, setText] = useState("default");
    const [modal, setModal] = useState(false);

    return (
        <div className="App">
            <ModalWindow visible={modal} setVisible={setModal}>
                <IssueForm  setVisible={setModal}/>
            </ModalWindow>
            <header>
                <div className="header-left-section">
                    <h2>Task Management system</h2>
                </div>
                <div className="header-right-section">
                    <button
                        className="btn btn-primary"
                        onClick={() => setModal(true)}>
                        Create issue
                    </button>
                </div>
            </header>
            <main>
                <div className="tree-container">
                    <TreeView />
                </div>
                <div className="content">
                    <IssueView />
                </div>
            </main>
        </div>
    );
}

export default App;
