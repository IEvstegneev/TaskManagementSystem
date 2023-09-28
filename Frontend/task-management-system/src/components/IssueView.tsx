import React, { useEffect, useState } from "react";
import IssuesService from "../Api/IssuesService";
import { useFetching } from "../hooks/useFetching";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { selectCurrentIssueId } from "../store/slices/issueSlice";
import { IIssue } from "../interfaces/IIssue";

function IssueView() {
    const dispatch = useAppDispatch();
    const currentId = useAppSelector(selectCurrentIssueId);
    const [issue, setIssue] = useState<IIssue>();

    const [fetchIssue, isLoading, error] = useFetching(async () => {
        const issue = await IssuesService.getIssue(currentId);
        setIssue(issue);
        console.log(issue);
    });

    useEffect(() => {
        if (currentId.length > 0) fetchIssue();
    }, [currentId]);

    return (
        <>
            {issue ? (
                <div className="container">
                    <h1>{issue?.title}</h1>
                    <p>Description</p>
                </div>
            ) : (
                <></>
            )}
        </>
    );
}

export default IssueView;
