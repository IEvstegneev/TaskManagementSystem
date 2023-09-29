import "../styles/IssueView.css";
import React, { useEffect, useState } from "react";
import IssuesService from "../Api/IssuesService";
import { useFetching } from "../hooks/useFetching";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import {
    resetCurrentId,
    selectCurrentIssueId,
    setParentId,
} from "../store/slices/issueSlice";
import { IIssue } from "../interfaces/IIssue";

function IssueView() {
    const dispatch = useAppDispatch();
    const currentId = useAppSelector(selectCurrentIssueId);
    const [isEditMode, setIsReadMode] = useState(false);
    const [issue, setIssue] = useState<IIssue>();

    const [title, setTitle] = useState<string>();
    const [performers, setPerformers] = useState<string>();
    const [description, setDescription] = useState<string>();

    const [fetchIssue, isFetching, fetchError] = useFetching(async () => {
        const issue = await IssuesService.getIssue(currentId as string);
        setIssue(issue);
        setTitle(issue.title);
        setPerformers(issue.performers);
        setDescription(issue.description);
    });

    useEffect(() => {
        if (currentId) fetchIssue();
    }, [currentId]);

    const editIssue = () => setIsReadMode(true);
    const cancelEditing = () => {
        setIsReadMode(false);
        setTitle(issue?.title || "");
        setPerformers(issue?.performers || "");
        setDescription(issue?.description || "");
    };

    const [updateIssue, isUpdating, updateError] = useFetching(async () => {
        await IssuesService.updateIssue({
            title,
            performers,
            description,
        });
        setIsReadMode(false);
    });
    const [deleteIssue, isDeleting, deleteError] = useFetching(async () => {
        if (issue){
            await IssuesService.deleteIssue(issue.id);
            resetCurrentId();
        }
    });


    const addNewIssue = () => {
        dispatch(resetCurrentId());
        dispatch(setParentId(issue?.id as string));
    };

    return (
        <>
            {isFetching ? (
                <h1>Loading...</h1>
            ) : (
                <div className="issueView">
                    <span className="date">дата: {issue?.createAt}</span>
                    <div className="issueForm">
                        <div className="mb-3">
                            <label>
                                Наименование
                                <input
                                    type="text"
                                    className="title"
                                    value={title}
                                    onChange={(event) =>
                                        setTitle(event.target.value)
                                    }
                                    readOnly={!isEditMode}
                                />
                            </label>
                        </div>
                        <div className="mb-3">
                            <label>
                                Исполнители
                                <input
                                    type="text"
                                    className="performers"
                                    value={performers}
                                    onChange={(event) =>
                                        setPerformers(event.target.value)
                                    }
                                    readOnly={!isEditMode}
                                />
                            </label>
                        </div>
                        <div className="mb-3">
                            <label>
                                Описание
                                <textarea
                                    className="description form-control-plaintext"
                                    value={description}
                                    onChange={(event) =>
                                        setDescription(event.target.value)
                                    }
                                    rows={5}
                                    maxLength={2000}
                                    readOnly={!isEditMode}></textarea>
                            </label>
                        </div>
                    </div>

                    <div>
                        {isEditMode ? (
                            <div className="issueBtnContainer">
                                <button
                                    type="button"
                                    className="btn btn-primary"
                                    onClick={updateIssue}>
                                    Сохранить
                                </button>
                                <button
                                    type="button"
                                    className="btn btn-danger"
                                    onClick={cancelEditing}>
                                    Отмена
                                </button>
                            </div>
                        ) : (
                            <div>
                                <div className="issueBtnContainer">
                                    <button
                                        type="button"
                                        className="btn btn-outline-primary"
                                        onClick={editIssue}>
                                        Редактировать
                                    </button>
                                    <button
                                        type="button"
                                        className="btn btn-outline-danger"
                                        onClick={deleteIssue}>
                                        Удалить
                                    </button>
                                </div>
                                <button
                                    type="button"
                                    className="btn btn-success"
                                    onClick={addNewIssue}>
                                    Добавить подзадачу
                                </button>

                                <div id="childIssueList" className="list-group">
                                    {issue?.children?.map((child) => (
                                        <button
                                            key={child.id}
                                            type="button"
                                            className="list-group-item list-group-item-action">
                                            {child.title}
                                        </button>
                                    ))}
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            )}
        </>
    );
}

export default IssueView;
