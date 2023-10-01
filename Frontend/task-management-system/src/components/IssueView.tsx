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
import { IssueStatus } from "../interfaces/IssueStatus";

function IssueView() {
    const dispatch = useAppDispatch();
    const currentId = useAppSelector(selectCurrentIssueId);
    const [isEditMode, setIsReadMode] = useState(false);
    const [issue, setIssue] = useState<IIssue>();
    const [title, setTitle] = useState<string>();
    const [performers, setPerformers] = useState<string>();
    const [description, setDescription] = useState<string>();
    const [status, setStatus] = useState(IssueStatus.Assigned);
    const [canStart, setCanStart] = useState(false);
    const [canStopOrFinish, setCanStopOrFinish] = useState(false);
    const [createAt, setCreateAt] = useState("123");

    const [fetchIssue, isFetching, fetchError] = useFetching(async () => {
        const response = await IssuesService.getIssue(currentId as string);
        setIssue(response);
        setCreateAt(response.createdAt);
        setStatus(response.status);
        setTitle(response.title);
        setPerformers(response.performers);
        setDescription(response.description);
        setCanStart(
            response.status === IssueStatus.Assigned ||
                response.status === IssueStatus.Stopped
        );
        setCanStopOrFinish(response.status === IssueStatus.InProgress);
    });

    useEffect(() => {
        if (currentId) fetchIssue();
    }, [currentId, status]);

    const editIssue = () => setIsReadMode(true);
    const cancelEditing = () => {
        setIsReadMode(false);
        setTitle(issue?.title || "");
        setPerformers(issue?.performers || "");
        setDescription(issue?.description || "");
    };

    const [updateIssue] = useFetching(async () => {
        if (issue)
            await IssuesService.updateIssue(issue?.id, {
                title,
                performers,
                description,
            });
        setIsReadMode(false);
    });
    const [deleteIssue] = useFetching(async () => {
        if (issue) {
            await IssuesService.deleteIssue(issue.id);
            dispatch(resetCurrentId());
        }
    });

    const addNewIssue = () => {
        dispatch(resetCurrentId());
        dispatch(setParentId(issue?.id as string));
    };

    const [startIssue] = useFetching(async () => {
        if (issue) {
            setStatus(IssueStatus.InProgress);
            setCanStopOrFinish(true);
            setCanStart(false);
            await IssuesService.startIssue(issue.id);
        }
    });
    const [stopIssue] = useFetching(async () => {
        if (issue) {
            setStatus(IssueStatus.Stopped);
            setCanStart(true);
            setCanStopOrFinish(false);
            await IssuesService.stopIssue(issue.id);
        }
    });
    const [finishIssue] = useFetching(async () => {
        if (issue) {
            setStatus(IssueStatus.Finished);
            setCanStopOrFinish(false);
            setCanStart(false);
            await IssuesService.finishIssue(issue.id);
        }
    });

    return (
        <>
            {isFetching ? (
                <h1>Loading...</h1>
            ) : (
                <div className="issueView">
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
                                    rows={3}
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
                                <div className="issueStatusContainer">
                                    <h6>
                                        Статус задачи: {IssueStatus[status]}
                                    </h6>
                                    <h6>
                                        Дата регистрации задачи:{" "}
                                        {issue?.createdAt}
                                    </h6>
                                    <h6>
                                        Дата завершения задачи:{" "}
                                        {issue?.finishedAt}
                                    </h6>
                                    <h6>
                                        Плановая трудоёмкость:{" "}
                                        {issue?.estimatedLaborCost}
                                    </h6>
                                    <h6>
                                        Фактическое время выполнения:{" "}
                                        {issue?.actualLaborCost}
                                    </h6>
                                </div>
                                <div className="issueBtnContainer">
                                    <button
                                        type="button"
                                        className="btn btn-primary"
                                        disabled={!canStart}
                                        onClick={startIssue}>
                                        Начать
                                    </button>
                                    <button
                                        type="button"
                                        className="btn btn-secondary"
                                        disabled={!canStopOrFinish}
                                        onClick={stopIssue}>
                                        Приостановить
                                    </button>

                                    <button
                                        type="button"
                                        className="btn btn-dark"
                                        disabled={!canStopOrFinish}
                                        onClick={finishIssue}>
                                        Завершить
                                    </button>
                                </div>
                                <div id="childIssueList" className="list-group">
                                    <span>Подзадачи</span>
                                    {issue?.children?.map((child) => (
                                        <button
                                            key={child.id}
                                            type="button"
                                            className="list-group-item list-group-item-action">
                                            {child.title}
                                        </button>
                                    ))}
                                </div>
                                <button
                                    type="button"
                                    className="btn btn-success"
                                    onClick={addNewIssue}>
                                    Добавить подзадачу
                                </button>
                            </div>
                        )}
                    </div>
                </div>
            )}
        </>
    );
}

export default IssueView;
