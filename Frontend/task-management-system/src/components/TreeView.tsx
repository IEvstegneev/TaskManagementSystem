import React, { useEffect, useState } from "react";
import { TreeItem } from "./TreeItem";
import { ITreeItem } from "../interfaces/ITreeItem";
import "../styles/TreeView.css";
import IssuesService from "../Api/IssuesService";
import { useFetching } from "../hooks/useFetching";
import {
    movingIssuesId,
    register,
    unRegister,
} from "../store/slices/movingSlice";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import {
    resetCurrentId,
    resetParentId,
    selectCurrentIssueId,
} from "../store/slices/issueSlice";

function TreeView() {
    const [items, setItems] = useState<ITreeItem[]>([]);
    const [fetchIssues, isLoading, error] = useFetching(async () => {
        const issues = await IssuesService.getIssuesList();
        setItems(issues);
    });

    const currentId = useAppSelector(selectCurrentIssueId);
    const ids = useAppSelector(movingIssuesId);
    useEffect(() => {
        fetchIssues();
    }, [ids]);

    const dispatch = useAppDispatch();
    const allowDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
    };
    const dropHandler = async (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault();
        const id = event.dataTransfer.getData("text");
        dispatch(register(id));
        await IssuesService.moveIssueToRoot(id);
        dispatch(unRegister(id));
    };

    const createIssue = () => {
        dispatch(resetParentId());
        dispatch(resetCurrentId());
    };

    return (
        <>
            <h4 onDragOver={allowDrop} onDrop={dropHandler}>
                Дерево задач
            </h4>
            {error && <h4>Error loading</h4> }
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
                        Обновить
                    </button>
                    <button className="btn btn-primary" onClick={createIssue}>
                        Создать задачу
                    </button>
                </div>
            )}
        </>
    );
}

export default TreeView;
