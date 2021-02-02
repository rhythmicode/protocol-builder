import { AbstractReturn } from './AbstractReturn';
interface ApiReturnInterface {
    Payload():         AbstractReturn;
    Meta(): string;
    Fill(p1: string | null, p2: Array<number>, p3: Array<number>, p4: number | null): string;
}
