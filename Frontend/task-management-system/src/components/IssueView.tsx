import "../styles/IssueView.css";
import React, { useEffect, useState } from "react";
import IssuesService from "../Api/IssuesService";
import { useFetching } from "../hooks/useFetching";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { selectCurrentIssueId } from "../store/slices/issueSlice";
import { IIssue } from "../interfaces/IIssue";

function IssueView() {
    const dispatch = useAppDispatch();
    const [readMode, setReadMode] = useState(true);
    const currentId = useAppSelector(selectCurrentIssueId);
    const [issue, setIssue] = useState<IIssue>();

    const [fetchIssue, isLoading, error] = useFetching(async () => {
        const issue = await IssuesService.getIssue(currentId);
        setIssue(issue);
    });

    useEffect(() => {
        if (currentId.length > 0) fetchIssue();
    }, [currentId]);

    return (
        <>
            {issue ? (
                <div className="issueView">
                    <div className="mb-3">
                        <span className="date">дата: {issue.createAt}</span>
                        <label>
                            Наименование
                            <input
                                type="text"
                                readOnly={readMode}
                                className="title"
                                value={issue?.title}
                            />
                        </label>
                    </div>
                    <div className="mb-3">
                        <label>
                            Исполнители
                            <input
                                type="text"
                                readOnly={readMode}
                                className="performers"
                                value={issue?.performers}
                                placeholder="Вася"
                            />
                        </label>
                    </div>
                    <div className="mb-3">
                        <label>
                            Описание
                            <textarea
                                readOnly={readMode}
                                className="description form-control-plaintext"
                                value={issue?.description}
                                placeholder="WARNING in [eslint]
                                src\components\IssueForm.tsx
                                  Line 6:12:  'id' is assigned a value but never used         @typescript-eslint/no-unused-vars
                                  Line 8:25:  'isLoading' is assigned a value but never used  @typescript-eslint/no-unused-vars
                                  Line 8:36:  'error' is assigned a value but never used      @typescript-eslint/no-unused-vars
                                Compiled with warnings."
                                rows={5}></textarea>
                        </label>
                    </div>

                    <div className="issueBtnContainer">
                        <button type="button" className="btn btn-primary">
                            Редактировать
                        </button>
                        <button type="button" className="btn btn-danger">
                            Удалить
                        </button>
                    </div>

                    <button type="button" className="btn btn-outline-success">
                        Добавить подзадачу
                    </button>

                    <div id="childIssueList" className="list-group">
                        {issue.children?.map((child) => (
                            <button
                                type="button"
                                className="list-group-item list-group-item-action">
                                {child.title}
                            </button>
                        ))}
                    </div>
                </div>
            ) : (
                <></>
            )}
        </>
    );
}

export default IssueView;
