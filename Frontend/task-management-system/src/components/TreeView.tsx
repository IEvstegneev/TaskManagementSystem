import React, { useEffect, useState } from "react";
import { TreeItem } from "./TreeItem";
import { ITreeItem } from "../interfaces/ITreeItem";
import "../styles/Tree.css";
import IssuesService from "../Api/IssuesService";
import { useFetching } from "../hooks/useFetching";

function TreeView() {
    const [items, setItems] = useState<ITreeItem[]>([]);
    const [fetchIssues, isLoading, error] = useFetching(async () => {
        const issues = await IssuesService.getIssuesList();
        setItems(issues);
    });

    useEffect(() => {
        fetchIssues();
    }, []);

    return (
        <>
            {error && <h4>Error loading</h4>}
            {isLoading ? (
                <label>
                    <h4 className="sr-only">Loading...</h4>
                    <div className="spinner-border" role="status" />
                </label>
            ) : (
                <div className="tree-view">
                    <ul>
                        {items.map((item) => (
                            <TreeItem data={item} key={item.id} />
                        ))}
                    </ul>
                    <button className="btn btn-secondary" onClick={fetchIssues}>
                        Retry
                    </button>
                </div>
            )}
        </>
    );
}

export default TreeView;
