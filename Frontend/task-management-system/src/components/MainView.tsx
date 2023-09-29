import "../styles/IssueView.css";
import React from "react";
import { useAppSelector } from "../store/hooks";
import {
    selectCurrentIssueId,
    selectParentIssueId,
} from "../store/slices/issueSlice";
import IssueView from "./IssueView";
import CreateIssueView from "./CreateIssueView";

function MainView() {
    const currentId = useAppSelector(selectCurrentIssueId);
    const parentId = useAppSelector(selectParentIssueId);

    return (
        <>
            {currentId ? (
                <IssueView />
            ) : (
                <CreateIssueView parentId={parentId} />
            )}
        </>
    );
}

export default MainView;
