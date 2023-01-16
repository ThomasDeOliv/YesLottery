import { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import style from './Result.module.css';
import MoneyW from './money_white.png';

function Result() {

    // HOOKS *************************************************************************************

    // Hook for navigation
    let navigate = useNavigate();

    // Hook for location
    const location = useLocation();

    // Hook to get the rank
    const [rank, setRank] = useState(5);

    // Hook to get the played numbers
    const [playedNumb, setPlayedNumb] = useState([]);

    // Hook to get the drawed numbers
    const [drawedNumb, setDrawedNumb] = useState([]);

    // Hook to get the date of the user game
    const [gameDateTime, setGameDateTime] = useState(new Date());

    // Hook to get the end date of the related draw
    const [endDateTime, setEndDateTime] = useState(new Date());

    // Hook to get the statistics of the related draw
    const [statistics, setStatistics] = useState({rank1: 0, rank2: 0, rank3: 0, rank4: 0, rank5: 0});

    // Fetch Datas for the relative ticket
    useEffect(() => {

        try{

            let playedNumbers = location.state.datas.playedNumbers;
            setPlayedNumb(playedNumbers.split(','));

            let drawedNumbers = location.state.datas.draw.drawedNumbers;   
            setDrawedNumb(drawedNumbers.split(','));

            let rankNumber = location.state.datas.gameRank;   
            setRank(rankNumber);

            let playDateTime = location.state.datas.gameDateTime;    
            setGameDateTime(new Date(playDateTime));

            let closureDateTime = location.state.datas.draw.endDateTime;     
            setEndDateTime(new Date(closureDateTime));
            
            let stats = location.state.datas.draw.statistics; 
            setStatistics(stats);

        } catch {

            navigate('/error', { state: { status: 500 } });

        }

    }, [location, navigate]);

    // FUNCTIONS *********************************************************************************

    // Function to render the spheres components
    const renderSpheres = (playedNumbers, drawedNumbers, isForDraw) => {

        if (isForDraw) {

            let renderedNumbers = [];

            drawedNumbers.forEach(element => {
                renderedNumbers.push(<div className={style.sphere} key={element}>{element}</div>);
            });

            return renderedNumbers;

        } else {

            let renderedNumbers = [];

            playedNumbers.forEach(element => {
                renderedNumbers.push(<div className={style.sphere} key={element} style={drawedNumbers.includes(element) ? { color: "#000000" } : { color: "#FA8F89" }}>{element}</div>);
            });

            return renderedNumbers;
        }
    };

    // Function to render all the possible results lines
    const renderResult = () => {

        // Function to return the gain of the user
        const calculateCabbage = (rankNum) => {

            let prize = 10 + statistics.rank1 + statistics.rank2 + statistics.rank3 + statistics.rank4 + statistics.rank5;
            let gains = 0;

            switch(rankNum) {

                case 1:
                    gains = (0.6 * prize)/statistics.rank1;
                    break;
                
                case 2:
                    gains = (0.2 * prize)/statistics.rank2;
                    break;

                case 3:
                    gains = (0.2 * prize)/statistics.rank3;
                break;

                default:
                    gains = 0;
                    break;
            }

            return Math.trunc(gains);
        }; 

        // Element if the user win a gain
        let displayElement = null;

        // First sentence to render
        let firstSentence = "";

        // Second sentence to render
        let secondSentence = "";

        switch(rank){

            case 1 : 
                firstSentence = 'Félicitations ! Vous avez trouvé les 6 numéros qui ont étés tirés.';
                secondSentence = "Cette combinaison vous a fait remporter : ";
                displayElement = <span><p>{calculateCabbage(rank)}</p><img src={MoneyW} alt="Cabbage symbol"/></span>;
                break;
            
            case 2 : 
                firstSentence = "Félicitations ! Vous avez trouvé 5 des 6 numéros qui ont étés tirés.";
                secondSentence = "Cette combinaison vous a fait remporter : ";
                displayElement = <span><p>{calculateCabbage(rank)}</p><img src={MoneyW} alt="Cabbage symbol"/></span>;
                break;    

            case 3 : 
                firstSentence = "Félicitations ! Vous avez trouvé 4 des 6 numéros qui ont étés tirés.";
                secondSentence = "Cette combinaison vous a fait remporter : ";
                displayElement = <span><p>{calculateCabbage(rank)}</p><img src={MoneyW} alt="Cabbage symbol"/></span>;
                break;

            default:
                firstSentence = "Malheureusement, cette combinaison ne vous a pas porté chance...";
                secondSentence = "Ne vous découragez pas et rententez votre chance lors du prochain tirage !";
                displayElement = null;
                break;
        };

        return (
            <>
                <p>{firstSentence}</p>
                <p>{secondSentence}</p>
                <div id={style["gains"]} style={rank === 4 ? {border: '0px'} : {display: '3px solid white'}}>{displayElement}</div>
            </>
        );
    };

    // Function to render a date string
    const renderDate = (datetime) => {

        let day = datetime.getDay() < 10 ? '0' + datetime.getDay().toString() : datetime.getDay().toString();
        let month = datetime.getMonth() + 1 < 10 ? '0' + (datetime.getMonth() + 1) : (datetime.getMonth() + 1);
        let year = datetime.getFullYear() < 10 ? '0' + datetime.getFullYear().toString() : datetime.getFullYear().toString();

        return `${day}/${month}/${year}`;
    };

    // Function to render a time string
    const renderTime = (datetime) => {

        let hour = datetime.getHours() < 10 ? '0' + datetime.getHours().toString() : datetime.getHours().toString();
        let minutes = datetime.getMinutes() < 10 ? '0' + datetime.getMinutes().toString() : datetime.getMinutes().toString();

        return `${hour}:${minutes}`;
    };

    // RENDERING *********************************************************************************

    return (
        <section className={style.result}>
            <h2>Résultats</h2>
            <div>
                <h3>Tirage</h3>
                <p>Les numéros ci-dessous sont ceux qui correspondent au tirage clôturé le {renderDate(endDateTime)} à {renderTime(endDateTime)}.</p>
                <div className={style.spheresContainer}>
                    {renderSpheres(playedNumb, drawedNumb, true).map(item => item)}
                </div>
            </div>
            <div>
                <h3>Votre jeu</h3>
                <p>Les numéros ci-dessous sont ceux que vous avez joué pour ce tirage le {renderDate(gameDateTime)} à {renderTime(gameDateTime)}.</p>
                <div className={style.spheresContainer}>
                    {renderSpheres(playedNumb, drawedNumb, false).map(item => item)}
                </div>
                <div>
                    {renderResult()}
                </div>
                <button onClick={() => navigate('/')}>Accueil</button>
            </div>
        </section>
    );
}

export default Result;