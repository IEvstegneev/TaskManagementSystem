import { ITreeItem } from "./ITreeItem";

export interface IIssue {
    id: number;
    title: string;
    description: string;
    performers: string;
    createAt: string;

    children: ITreeItem[];
}
