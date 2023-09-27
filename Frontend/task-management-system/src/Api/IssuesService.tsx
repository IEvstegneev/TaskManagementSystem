import axios from "axios";
import { ITreeItem } from "../interfaces/ITreeItem";
import { ICreateIssueRequest } from "../interfaces/ICreateIssueRequest";

export default class IssuesService {
    static async getIssuesList() {
        const { data } = await axios.get<ITreeItem[]>(
            "https://localhost:7081/issues/"
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
}
