import { ITreeItem } from "./ITreeItem";

export interface IIssue {
    id: string;
    title: string;
    description: string;
    performers: string;
    createAt?: string;

    children?: ITreeItem[];
}
