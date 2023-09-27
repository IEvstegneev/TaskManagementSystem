import React, { Dispatch, SetStateAction } from "react";
import cl from "./ModalWindow.module.css";

function ModalWindow({
    children,
    visible,
    setVisible,
}: {
    children: JSX.Element;
    visible: boolean;
    setVisible: Dispatch<SetStateAction<boolean>>;
}) {
    const rootClasses = [cl.modalWindow];
    if (visible) {
        rootClasses.push(cl.active);
    }

    return (
        <div className={rootClasses.join(" ")}>
            <div className={cl.modalWindowContent}>
                <button className="btn" onClick={() => setVisible(false)}>
                    X
                </button>
                {children}
            </div>
        </div>
    );
}

export default ModalWindow;
