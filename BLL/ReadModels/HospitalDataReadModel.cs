using DAL.Models;

namespace BLL.ReadModels
{
    public record Label(string label);
    public record Data(int value);
    public record DataSet(string seriesname, List<Data> data);

    public record Chart(
        string caption,
        string subCaption,
        string numberPrefix,
        string theme,
        string radarfillcolor,
        string xAxisName,
        string yAxisNamevenues
        );

    public record Category(List<Label> category);

    public record HospitalDataReadModel(
        Chart chart,
        List<Category> categories,
        List<DataSet> dataset
        );
}
