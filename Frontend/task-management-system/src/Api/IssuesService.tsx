import axios from "axios";
import { ITreeItem } from "../interfaces/ITreeItem";
import { ICreateIssueRequest } from "../interfaces/ICreateIssueRequest";
import { IIssue } from "../interfaces/IIssue";

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
    static async postIssue(issue: ICreateIssueRequest) {
        const { data, status } = await axios.post<string>(
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

    static async moveIssue(fromId: string, toId: string) {
        const { status } = await axios.get(
            `https://localhost:7081/issues/${fromId}/move`,
            {
                params: {
                    to: toId,
                },
                headers: {
                    Accept: "application/json",
                },
            }
        );
        return status === 204;
    }
}
