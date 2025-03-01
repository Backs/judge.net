namespace Judge.Compiler;

public interface ICompiler
{
    CompileResult Compile(CompileSource sourceCode, string workingDirectory);
}