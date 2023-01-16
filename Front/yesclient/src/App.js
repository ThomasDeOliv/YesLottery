import './App.css';
import { Routes, Route, useNavigate, Navigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import parser from 'cron-parser';
import React from 'react';
import Loading from './Components/Loading/Loading';
import Header from './Components/Header/Header';
import Footer from './Components/Footer/Footer';
import Modal from './Components/Modal/Modal';
import ErrorComponent from './Components/InnerComponents/ErrorComponent/ErrorComponent';
import Home from './Components/InnerComponents/Home/Home';
import Play from './Components/InnerComponents/Play/Play';
import Registration from './Components/InnerComponents/Registration/Registration';
import Result from './Components/InnerComponents/Result/Result';
import Rules from './Components/InnerComponents/Rules/Rules';
import Mentions from './Components/InnerComponents/Mentions/Mentions';

function App() {

    // HOOKS *************************************************************************************

    // Navigation hook
    let navigate = useNavigate();

    // Error message for accessCode
    const [errorMessage, setErrorMessage] = useState('');

    // Content of the modal and homepage input
    const [accessCode, setAccessCode] = useState('');

    // Properties to close the modal window
    const [closedModal, setClosedModal] = useState(true);

    // Determine if the app is loading datas
    const [isLoading, setIsLoading] = useState(true);

    // State of the current draw (open/closed)
    const [currentDrawClosed, setCurrentdrawClosed] = useState(true);

    // Timer
    const [timer, setTimer] = useState('00:00');

    // Number of cabbage for the current draw
    const [currentParticipants, setCurrentParticipants] = useState(10);

    // Detection of the size of screen (true if mobile screen)
    const [screenMobile, setMobileScreen] = useState(undefined);

    // BACKGROUNDSERVICES ************************************************************************

    // TIMER
    useEffect(() => {

        // Calling the API to get the draw informations
        const getCurrentDraw = async () => {    
            
            // URL to contact
            const url = process.env.REACT_APP_API_ENDPOINT_DRAWS;

            // Options
            const options = { 

                method: 'GET', 
                headers: { 
                    "Content-type": "text/plain; charset=UTF-8",
                    "Access-Control-Allow-Origin": process.env.REACT_APP_ORIGIN_URL
                }
            };

            try {
                // Fetch API
                let data = await fetch(url, options)
                    .then(response => {

                        if (!response.ok || response.status === 204) {
                            throw new ErrorComponent(response.status);
                        }

                        return response.text();
                    });

                setCurrentParticipants(parseInt(data) + 10);
                setIsLoading(false);

            } catch (error) {

                throw new Error(error.message);
            }
        };

        // Get and convert all dates
        const renderCron = () => {

            const cronExpression = "0 0/5 * * * ?";

            const options = {

                currentDate: new Date(),
                iterator: true
            };

            const interval = parser.parseExpression(cronExpression, options);

            const nextDateTime = interval.next().value.toDate();

            const rawDifference = new Date(nextDateTime - new Date());

            const display = `${rawDifference.getMinutes() < 10 ? '0' + rawDifference.getMinutes() : rawDifference.getMinutes()}:${rawDifference.getSeconds() < 10 ? '0' + rawDifference.getSeconds() : rawDifference.getSeconds()}`;

            if (rawDifference.getMinutes() < 1) {

                setCurrentdrawClosed(true);

            } else {

                setCurrentdrawClosed(false);

            }

            return display;
        };

        // Contact the API each 7,5 seconds
        const contactApi = setInterval(() => 
        {
            getCurrentDraw();
        },
            1500);

        // Action to execute each seconds
        const renderTimer = setInterval(() =>
        {
            let result = renderCron();
            setTimer(result);
        },
            1000);

        // Return the clear interval where we fetch the api each 5 seconds
        return () => {

            clearInterval(renderTimer);
            clearInterval(contactApi);
        }

    }, [navigate]);

    // MODAL WINDOW
    useEffect(() => {

        if (!isLoading) {

            const links = document.querySelectorAll('a');
            links.forEach(link => link.addEventListener('click', () => {

                setClosedModal(true);

            }));

            const buttons = document.querySelectorAll('nav > ul > li > ul > li > button');
            buttons.forEach(button => button.addEventListener('click', () => {

                setClosedModal(true);

            }));
        }

    }, [isLoading]);

    // DISAPEARANCE OF ERROR MESSAGE
    useEffect(() => {

        if(errorMessage){

            setTimeout(() => {

                setErrorMessage('');

            }, 3000);

        }

    }
    , [errorMessage]);

    //DETECTION OF THE SCREEN SIZE - TO DISABLE WHEN FINISHING THE MOBILE HEADER CONSTRUCTION
    useEffect(() => {
        
        if(screenMobile === undefined) {

            setMobileScreen(window.innerWidth < 1315 ? true : false);
            
        } else {

            window.addEventListener("resize", () => {

                if(window.innerWidth < 1315) {
    
                    setMobileScreen(true);
    
                } else {
    
                    setMobileScreen(false);
                }
            });

        }

    }, [screenMobile]);

    // FUNCTIONS *********************************************************************************

    // Check access code from modal or home page
    const navToResult = async (accessCode) => {

        // URL to contact
        const url = `${process.env.REACT_APP_API_ENDPOINT_RESULTS}/${accessCode}`;

        // Options
        const options = { 

            method: 'GET', 
            headers: { 
                "Content-type": "text/plain; charset=UTF-8",
                "Access-Control-Allow-Origin": process.env.REACT_APP_ORIGIN_URL
            }
        };

        try {

            // Fetch DATAS
            let datas = await fetch(url, options)
                .then(response => {

                    if (!response.ok) {

                        throw new Error(response.status);
                    }

                    return response.json();
                });

            setClosedModal(true);
            setAccessCode('');
            navigate('/result', { state: { datas: datas } });

        } catch(error) {

            if (error.message === '401' || error.message === '403' || error.message === '404') {

                const elements = document.querySelectorAll('.errorModal');                
                elements.forEach(element => {

                    element.style.display = 'block';

                });

                setErrorMessage(error.message === '404' ? 'Le code d\'accès que vous avez saisi est incorrect.' : 'Vous ne pouvez pas encore accéder aux résultats de ce tirage.');

            } else {

                navigate('/error', { state: { status: error.message } });
                setClosedModal(true);
                setAccessCode('');
            }
        }
    };

    // RENDERING *********************************************************************************

    // Render function
    const render = (loading) => {

        // If the first fetch is loading, app send the loading page to visitors
        if (loading) {

            return (<Loading />);

        } else { // Else, it render the homepage

            return (
                <>
                    <Header timer={timer} currentParticipants={currentParticipants} currentDrawClosed={currentDrawClosed} setClosedModal={setClosedModal} screenMobile={screenMobile}/>
                    <main>
                        <Routes>
                            <Route path="/" element={<Home timer={timer} currentDrawClosed={currentDrawClosed} accessCode={accessCode} setAccessCode={(e) => setAccessCode(e.target.value)} errorMessage={errorMessage} navToResult={() => navToResult(accessCode)} />} />
                            <Route path="/play" element={<Play currentDrawClosed={currentDrawClosed} setClosedModal={setClosedModal} />} />
                            <Route path="/registration" element={<Registration />} />
                            <Route path="/result" element={<Result />} />
                            <Route path="/rules" element={<Rules />} />
                            <Route path="/error" element={<ErrorComponent />} />
                            <Route path="/mentions" element={<Mentions />} />
                            <Route path="*" element={<Navigate replace to="/error" state={{status: 404}}/>} />
                        </Routes>
                        <Modal closedModal={closedModal} setClosedModal={setClosedModal} accessCode={accessCode} setAccessCode={(e) => setAccessCode(e.target.value)} errorMessage={errorMessage} navToResult={() => navToResult(accessCode)} />
                    </main>
                    <Footer screenMobile={screenMobile}/>
                </>
            );
        }
    };

    // Return of the App Component
    return render(isLoading);
}

export default App;