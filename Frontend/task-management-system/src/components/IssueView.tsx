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
import { IssueStatus, getDisplayStatusName } from "../interfaces/IssueStatus";
import { IssueItem } from "./IssueItem";

function IssueView() {
    const dispatch = useAppDispatch();
    const currentId = useAppSelector(selectCurrentIssueId);
    const [isEditMode, setIsReadMode] = useState(false);
    const [issue, setIssue] = useState<IIssue>();
    const [title, setTitle] = useState<string>();
    const [performers, setPerformers] = useState<string>();
    const [description, setDescription] = useState<string>();
    const [hours, setHours] = useState(0);
    const [status, setStatus] = useState(IssueStatus.Assigned);

    const [fetchIssue, isFetching, fetchError] = useFetching(async () => {
        const response = await IssuesService.getIssue(currentId as string);
        setIssue(response);
        setTitle(response.title);
        setPerformers(response.performers);
        setDescription(response.description);
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
                estimatedLaborCost: hours,
            });
        setIsReadMode(false);
        fetchIssue();
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
            await IssuesService.startIssue(issue.id);
            setStatus(IssueStatus.InProgress);
        }
    });
    const [stopIssue] = useFetching(async () => {
        if (issue) {
            await IssuesService.stopIssue(issue.id);
            setStatus(IssueStatus.Stopped);
        }
    });
    const [finishIssue] = useFetching(async () => {
        if (issue) {
            await IssuesService.finishIssue(issue.id);
            setStatus(IssueStatus.Finished);
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
                        <div className="mb-3">
                            {isEditMode && (
                                <label>
                                    Плановая трудоёмкость задачи, час
                                    <input
                                        type="number"
                                        className="estimatedLaborCost"
                                        value={hours}
                                        onChange={(event) =>
                                            setHours(event.target.valueAsNumber)
                                        }
                                    />
                                </label>
                            )}
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
                                        Статус задачи:{" "}
                                        <strong>
                                            {issue
                                                ? getDisplayStatusName(
                                                      issue.status
                                                  )
                                                : " - "}
                                        </strong>
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
                                        disabled={!issue?.canStart}
                                        onClick={startIssue}>
                                        Начать
                                    </button>
                                    <button
                                        type="button"
                                        className="btn btn-secondary"
                                        disabled={!issue?.canStop}
                                        onClick={stopIssue}>
                                        Приостановить
                                    </button>

                                    <button
                                        type="button"
                                        className="btn btn-dark"
                                        disabled={!issue?.canFinish}
                                        onClick={finishIssue}>
                                        Завершить
                                    </button>
                                </div>
                                <div id="childIssueList" className="list-group">
                                    <span>Подзадачи</span>
                                    {issue?.children?.map((child) => (
                                        <IssueItem
                                            data={child}
                                            key={child.id}
                                        />
                                    ))}
                                </div>
                                <button
                                    type="button"
                                    className="btn btn-success"
                                    onClick={addNewIssue}
                                    disabled={
                                        issue?.status === IssueStatus.Finished
                                    }>
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
