import axios, { AxiosError } from "axios";
import { ITreeItem } from "../interfaces/ITreeItem";
import { ICreateIssueRequest } from "../interfaces/ICreateIssueRequest";
import { IIssue } from "../interfaces/IIssue";
import { IUpdateIssueRequest } from "../interfaces/IUpdateIssueRequest";
import { IssueStatus } from "../interfaces/IssueStatus";

export default class IssuesService {
    static async getIssuesList() {
        const { data } = await axios.get<ITreeItem[]>(
            "https://localhost:7081/issues/"
        );
        return data;
    }

    static async getIssuesChildrenList(parentId: string) {
        const { data } = await axios.get<ITreeItem[]>(
            `https://localhost:7081/issues/${parentId}/children`,
            {
                headers: {
                    Accept: "application/json",
                },
            }
        );
        return data;
    }

    static async getIssue(id: string) {
        const { data } = await axios.get<IIssue>(
            `https://localhost:7081/issues/${id}`,
            {
                headers: {
                    Accept: "application/json",
                },
            }
        );
        return data;
    }

    static async postIssue(issue: ICreateIssueRequest) {
        const { data } = await axios.post<string>(
            "https://localhost:7081/issues/",
            issue,
            {
                headers: {
                    "Content-Type": "application/json",
                    Accept: "application/json",
                },
            }
        );
        return data;
    }

    static async updateIssue(id: string, issue: IUpdateIssueRequest) {
        const { status } = await axios.put(
            `https://localhost:7081/issues/${id}`,
            issue,
            {
                headers: {
                    "Content-Type": "application/json",
                    Accept: "application/json",
                },
            }
        );
        return status === 204;
    }

    static async deleteIssue(id: string) {
        const { status } = await axios.delete(
            `https://localhost:7081/issues/${id}`
        );
        return status === 204;
    }

    static async moveIssue(id: string, toId: string) {
        await axios.get(
            `https://localhost:7081/issues/${id}/move`,
            {
                params: {
                    to: toId,
                },
                headers: {
                    Accept: "application/json",
                },
            }
        ).catch((r) => console.log((r as AxiosError).response?.data));
    }

    static async moveIssueToRoot(id: string) {
        const { status } = await axios.get(
            `https://localhost:7081/issues/${id}/move-to-root`,
            {
                headers: {
                    Accept: "application/json",
                },
            }
        );
        return status === 204;
    }

    static async changeStatus(id: string, newStatus: IssueStatus) {
        const { status } = await axios.get(
            `https://localhost:7081/issues/${id}/change-status`,
            {
                params: {
                    status: newStatus,
                },
                headers: {
                    Accept: "application/json",
                },
            }
        );
        return status === 204;
    }

    static async finishIssue(id: string) {
        const { status } = await axios.get(
            `https://localhost:7081/issues/${id}/change-status`,
            {
                params: {
                    status: IssueStatus.Finished,
                },
                headers: {
                    Accept: "application/json",
                },
            }
        );
        return status === 204;
    }

    static async startIssue(id: string) {
        const { status } = await axios.get(
            `https://localhost:7081/issues/${id}/change-status`,
            {
                params: {
                    status: IssueStatus.InProgress,
                },
                headers: {
                    Accept: "application/json",
                },
            }
        );
        return status === 204;
    }

    static async stopIssue(id: string) {
        const { status } = await axios.get(
            `https://localhost:7081/issues/${id}/change-status`,
            {
                params: {
                    status: IssueStatus.Stopped,
                },
                headers: {
                    Accept: "application/json",
                },
            }
        );
        return status === 204;
    }
}
